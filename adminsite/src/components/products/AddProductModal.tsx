import React, { useEffect, useState, useCallback, useRef } from 'react';
import { useForm } from 'react-hook-form';

// Import the VariantTab component
import VariantTab from './VariantTab';
import { useAppSelector } from '../../redux/hooks/redux';
import axiosConfig from '../../redux/config/axios.config';

interface AddProduct {
  name: string;
  description: string;
  price: string;
  classifications: string;
  image: File[] | null;
  variants?: any[]; // Add variants to the product data
}

interface FormValues {
  name: string;
  description: string;
  price: string;
  classifications: string;
  image: FileList | null;
  variants?: any[]; // Add variants to the form values
}

interface Classification {
  id: number;
  name: string;
}

interface ProductModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (data: AddProduct) => Promise<void>;
  error: string | null;
}

enum ModalTab {
  PRODUCT = 'product',
  VARIANT = 'variant'
}

// Debounce utility function
const debounce = (func: (value: string) => void, wait: number): ((value: string) => void) => {
  let timeout: NodeJS.Timeout;
  return (...args: [string]) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => func(...args), wait);
  };
};

const ProductModal: React.FC<ProductModalProps> = ({ isOpen, onClose, onSubmit, error: serverError }) => {
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [clsft, setClsft] = useState<Classification[]>([]);
  const [selectedClassifications, setSelectedClassifications] = useState<Classification[]>([]);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [searchStatus, setSearchStatus] = useState<'idle' | 'loading' | 'success' | 'error'>('idle');
  const [searchError, setSearchError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<ModalTab>(ModalTab.PRODUCT);
  const [formValidated, setFormValidated] = useState<boolean>(false);
  
  // Reference to the classification container div for handling outside clicks
  const classificationContainerRef = useRef<HTMLDivElement>(null);
  const { classifications } = useAppSelector((state) => state.classifications);

  const {
    register,
    handleSubmit,
    reset,
    watch,
    setValue,
    formState: { errors, isValid }
  } = useForm<FormValues>({
    defaultValues: {
      name: '',
      description: '',
      price: '',
      classifications: '',
      image: null,
      variants: [] // Initialize variants as empty array
    },
    mode: 'onChange' // Enable real-time validation
  });

  const imageFile: FileList | null = watch('image');
  const allFieldsWatched = watch(['name', 'description', 'price', 'classifications', 'image']);
  
  // Get name and price for passing to VariantTab
  const productName = watch('name');
  const productPrice = watch('price');
  
  // Check if all product fields are valid
  const isProductFormValid = 
    !!allFieldsWatched[0] && // name
    !!allFieldsWatched[1] && // description
    !!allFieldsWatched[2] && // price
    !!allFieldsWatched[3] && // classifications
    !!allFieldsWatched[4] && // image
    isValid;

  // Update formValidated state whenever form validity changes
  useEffect(() => {
    if (isProductFormValid && !formValidated) {
      setFormValidated(true);
    }
  }, [isProductFormValid, formValidated]);

  // Function to search classifications directly
  const searchClassification = async (query: string) => {
    if (!query.trim()) {
      setClsft([]);
      return;
    }
    
    try {
      setSearchStatus('loading');
      if (classifications.length !== 0) {
        const result = classifications.filter((classification: Classification) =>
          classification.name.toLowerCase().includes(query.toLowerCase())
        );
        setClsft(result);
      } else {
        const result = await axiosConfig.get(`http://localhost:5113/api/Classification/search?pattern=${encodeURIComponent(query)}`);
        setClsft(result.data);
      }
      setSearchStatus('success');
      setSearchError(null);
    } catch (error) {
      setSearchStatus('error');
      setSearchError(error instanceof Error ? error.message : 'Unknown error occurred');
      setClsft([]);
    }
  };

  // Handle clicking outside the classifications container
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (classificationContainerRef.current && 
          !classificationContainerRef.current.contains(event.target as Node)) {
        setClsft([]);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Handle image preview when file is selected
  useEffect(() => {
    if (imageFile && imageFile[0]) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result as string);
      };
      reader.readAsDataURL(imageFile[0]);
    }
  }, [imageFile]);

  // Reset form and clear preview when modal closes
  useEffect(() => {
    if (!isOpen) {
      reset();
      setImagePreview(null);
      setClsft([]);
      setSelectedClassifications([]);
      setSearchTerm('');
      setActiveTab(ModalTab.PRODUCT);
      setFormValidated(false);
    }
  }, [isOpen, reset]);

  // Debounced API call
  const debouncedSearch = useCallback(
    debounce((value: string) => {
      searchClassification(value);
    }, 400),
    []
  );

  // Handle search input change
  useEffect(() => {
    debouncedSearch(searchTerm);
  }, [searchTerm, debouncedSearch]);

  // Update the hidden form field with comma-separated IDs
  useEffect(() => {
    const classificationIds = selectedClassifications.map(c => c.id).join(', ');
    setValue('classifications', classificationIds, { shouldValidate: true });
  }, [selectedClassifications, setValue]);

  // Handle classification selection
  const handleSelectClassification = (classification: Classification): void => {
    // Check if this classification is already selected
    if (!selectedClassifications.some(c => c.id === classification.id)) {
      setSelectedClassifications([...selectedClassifications, {
        id: classification.id,
        name: classification.name
      }]);
    }
    
    // Clear search input and results
    setSearchTerm('');
    setClsft([]);
  };

  // Handle removing a classification
  const handleRemoveClassification = (id: number): void => {
    setSelectedClassifications(selectedClassifications.filter(c => c.id !== id));
  };

  const handleNextClick = () => {
    if (isProductFormValid) {
      setFormValidated(true);
      setActiveTab(ModalTab.VARIANT);
    }
  };

  const handleFormSubmit = handleSubmit(async (data: FormValues): Promise<void> => {
    const formData: AddProduct = {
      ...data,
      image: data.image ? Array.from(data.image) : null,
      variants: data.variants
    };
    
    await onSubmit(formData);
  });

  const onDragOver = (e: React.DragEvent<HTMLDivElement>): void => {
    e.preventDefault();
  };

  const onDrop = (e: React.DragEvent<HTMLDivElement>): void => {
    e.preventDefault();
    if (e.dataTransfer.files && e.dataTransfer.files[0]) {
      const fileInput = document.getElementById('image-upload') as HTMLInputElement;
      if (fileInput) {
        const dataTransfer = new DataTransfer();
        dataTransfer.items.add(e.dataTransfer.files[0]);
        fileInput.files = dataTransfer.files;
        
        const event = new Event('change', { bubbles: true });
        fileInput.dispatchEvent(event);
      }
    }
  };

  // Validate variants when in variant tab
  const validateVariants = () => {
    const variants = watch('variants');
    return variants && variants.length > 0;
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg shadow-lg overflow-hidden w-4/5 h-[90%] flex flex-col">
        {/* Tab Navigation */}
        <div className="flex border-b border-gray-200">
          <div
            className={`px-6 py-3 font-medium text-center cursor-pointer ${
              activeTab === ModalTab.PRODUCT ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500'
            }`}
            onClick={() => setActiveTab(ModalTab.PRODUCT)}
          >
            Product
          </div>
          <div
            className={`px-6 py-3 font-medium text-center ${
              activeTab === ModalTab.VARIANT 
                ? 'text-blue-600 border-b-2 border-blue-600' 
                : (formValidated || isProductFormValid)
                  ? 'text-gray-500 cursor-pointer' 
                  : 'text-gray-300 cursor-not-allowed'
            }`}
            onClick={() => {
              if (formValidated || isProductFormValid) {
                setActiveTab(ModalTab.VARIANT);
              }
            }}
          >
            Variant
          </div>
        </div>

        <form onSubmit={handleFormSubmit} className="flex flex-col flex-1 overflow-hidden">
          {/* Product Form */}
          {activeTab === ModalTab.PRODUCT && (
            <div className="flex flex-col md:flex-row flex-1 overflow-auto">
              {/* Left side - Form fields */}
              <div className="w-full md:w-1/2 p-6 space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Name</label>
                  <input
                    type="text"
                    className={`mt-1 block w-full border ${errors.name ? 'border-red-500' : 'border-gray-300'} rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500`}
                    {...register("name", { 
                      required: "Name is required",
                      minLength: { value: 2, message: "Name must be at least 2 characters" }
                    })}
                  />
                  {errors.name && <p className="text-red-500 text-sm mt-1">{errors.name.message}</p>}
                </div>
                
                <div>
                  <label className="block text-sm font-medium text-gray-700">Description</label>
                  <textarea
                    className={`mt-1 block w-full border ${errors.description ? 'border-red-500' : 'border-gray-300'} rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500`}
                    {...register("description", { 
                      required: "Description is required"
                    })}
                  />
                  {errors.description && <p className="text-red-500 text-sm mt-1">{errors.description.message}</p>}
                </div>
                
                <div>
                  <label className="block text-sm font-medium text-gray-700">Price</label>
                  <input
                    type="text"
                    className={`mt-1 block w-full border ${errors.price ? 'border-red-500' : 'border-gray-300'} rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500`}
                    {...register("price", { 
                      required: "Price is required",
                      pattern: { value: /^\d+(\.\d{1,2})?$/, message: "Enter a valid price" },
                      validate: (value: string): string | true => parseFloat(value) > 0 || "Price must be greater than 0"
                    })}
                  />
                  {errors.price && <p className="text-red-500 text-sm mt-1">{errors.price.message}</p>}
                </div>
                
                <div className="relative" ref={classificationContainerRef}>
                  <label className="block text-sm font-medium text-gray-700">Classifications</label>
                  
                  {/* Classification suggestions dropdown positioned above the input */}
                  {clsft.length > 0 && (
                    <div className="absolute z-10 bottom-full left-0 right-0 mb-1 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto">
                      <ul className="py-1">
                        {clsft.map((classification: Classification) => (
                          <li 
                            key={classification.id} 
                            className="px-3 py-2 hover:bg-blue-100 cursor-pointer text-gray-700"
                            onClick={() => handleSelectClassification(classification)}
                          >
                            {classification.name}
                          </li>
                        ))}
                      </ul>
                    </div>
                  )}
                  
                  {searchStatus === 'loading' && (
                    <div className="absolute bottom-full left-0 mb-1">
                      <p className="text-gray-500 text-sm">Loading...</p>
                    </div>
                  )}
                  
                  {/* Hidden input to store comma-separated classification IDs */}
                  <input
                    type="hidden"
                    {...register("classifications", { 
                      required: "At least one classification is required",
                      validate: (value: string): string | true => 
                        value.trim() !== '' || "At least one classification is required"
                    })}
                  />
                  
                  {/* Custom input field with selected tags display */}
                  <div className={`mt-1 flex flex-wrap items-center border ${errors.classifications ? 'border-red-500' : 'border-gray-300'} rounded-md px-2 py-1 focus-within:ring-2 focus-within:ring-blue-500 min-h-10`}>
                    {selectedClassifications.map((classification) => (
                      <div key={classification.id} className="flex items-center bg-blue-100 text-blue-800 rounded-md px-2 py-1 m-1 text-sm">
                        {classification.name}
                        <button
                          type="button"
                          className="ml-1 text-blue-500 hover:text-blue-700 focus:outline-none"
                          onClick={() => handleRemoveClassification(classification.id)}
                        >
                          &times;
                        </button>
                      </div>
                    ))}
                    
                    <input
                      type="text"
                      value={searchTerm}
                      onChange={(e) => setSearchTerm(e.target.value)}
                      className="flex-grow border-none focus:outline-none focus:ring-0 min-w-20 py-1"
                      placeholder={selectedClassifications.length > 0 ? "" : "Search classifications..."}
                    />
                  </div>
                  
                  {searchError && <p className="text-red-500 text-sm mt-1">{searchError}</p>}
                  {errors.classifications && <p className="text-red-500 text-sm mt-1">{errors.classifications.message}</p>}
                </div>
              </div>
              
              {/* Right side - Image upload */}
              <div className="w-full md:w-1/2 p-6 bg-gray-50 border-l border-gray-200">
                <div 
                  className={`flex justify-center items-center border-2 border-dashed rounded-md h-64 ${errors.image ? 'border-red-500' : 'border-gray-300'}`}
                  onDragOver={onDragOver}
                  onDrop={onDrop}
                >
                  <div className="space-y-1 text-center p-6">
                    {imagePreview ? (
                      <div className="flex flex-col items-center">
                        <img 
                          src={imagePreview} 
                          alt="Product preview" 
                          className="max-h-40 max-w-full mb-2 object-contain" 
                        />
                        <p className="text-sm text-gray-600">Click or drag to replace image</p>
                      </div>
                    ) : (
                      <div className="flex flex-col items-center">
                        <svg 
                          className="mx-auto h-12 w-12 text-gray-400" 
                          stroke="currentColor" 
                          fill="none" 
                          viewBox="0 0 48 48" 
                          aria-hidden="true"
                        >
                          <path 
                            d="M28 8H12a4 4 0 00-4 4v20m32-12v8m0 0v8a4 4 0 01-4 4H12a4 4 0 01-4-4v-4m32-4l-3.172-3.172a4 4 0 00-5.656 0L28 28M8 32l9.172-9.172a4 4 0 015.656 0L28 28m0 0l4 4m4-24h8m-4-4v8m-12 4h.02" 
                            strokeWidth="2" 
                            strokeLinecap="round" 
                            strokeLinejoin="round" 
                          />
                        </svg>
                        <p className="mt-2 text-sm text-gray-600">
                          <label htmlFor="image-upload" className="font-medium text-blue-600 hover:text-blue-500 cursor-pointer">
                            Upload a file
                          </label> or drag and drop
                        </p>
                        <p className="text-xs text-gray-500">
                          PNG, JPG, GIF up to 10MB
                        </p>
                      </div>
                    )}
                    <input
                      id="image-upload"
                      type="file"
                      accept="image/*"
                      className="sr-only"
                      {...register("image", { 
                        required: "Product image is required" 
                      })}
                    />
                  </div>
                </div>
                {errors.image && <p className="text-red-500 text-sm mt-1">{errors.image.message}</p>}
              </div>
            </div>
          )}
          
          {/* Variant Form - Integrated VariantTab Component */}
          {activeTab === ModalTab.VARIANT && (
            <div className="p-6 flex-1 overflow-auto">
              <VariantTab
                productName={productName}
                productPrice={productPrice}
                productImagePreview={imagePreview}
                register={register}
                setValue={setValue}
                watch={watch}
                errors={errors}
              />
              
              {/* Display error for variants */}
              {errors.variants && (
                <p className="text-red-500 text-sm mt-2">{errors.variants.message}</p>
              )}
            </div>
          )}
          
          {serverError && (
            <div className="px-6 pb-4">
              <p className="text-red-500 text-sm">{serverError}</p>
            </div>
          )}
          
          {/* Bottom Action Buttons - Centered */}
          <div className="flex justify-center space-x-4 p-6 border-t border-gray-200">
            <button
              type="button"
              onClick={onClose}
              className="px-6 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-100"
            >
              Cancel
            </button>
            
            {activeTab === ModalTab.PRODUCT ? (
              <button
                type="button"
                onClick={(e) => {
                  e.preventDefault();
                  handleNextClick();
                }}
                disabled={!(formValidated || isProductFormValid)}
                className={`px-6 py-2 rounded-md ${
                  (formValidated || isProductFormValid)
                    ? 'bg-blue-600 hover:bg-blue-700 text-white' 
                    : 'bg-blue-300 cursor-not-allowed text-white'
                }`}
              >
                Next
              </button>
            ) : (
              <button
                type="submit"
                disabled={!validateVariants()}
                className={`px-6 py-2 rounded-md ${
                  validateVariants()
                    ? 'bg-blue-600 hover:bg-blue-700 text-white' 
                    : 'bg-blue-300 cursor-not-allowed text-white'
                }`}
              >
                Add Product
              </button>
            )}
          </div>
        </form>
      </div>
    </div>
  );
};

export default ProductModal;