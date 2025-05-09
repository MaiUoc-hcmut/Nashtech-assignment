import React, { useState, useEffect, useCallback, useRef } from 'react';
import { UseFormRegister, UseFormSetValue, UseFormWatch, FieldErrors } from 'react-hook-form';
import axiosConfig from '../../redux/config/axios.config';

// Mapping of Vietnamese letters to English letters
const vietnameseToEnglishMap: { [key: string]: string } = {
  'Ă': 'A', 'Â': 'A', 'Á': 'A', 'À': 'A', 'Ả': 'A', 'Ã': 'A', 'Ạ': 'A',
  'ă': 'a', 'â': 'a', 'á': 'a', 'à': 'a', 'ả': 'a', 'ã': 'a', 'ạ': 'a',
  'Ơ': 'O', 'Ô': 'O', 'Ó': 'O', 'Ò': 'O', 'Ỏ': 'O', 'Õ': 'O', 'Ọ': 'O',
  'ơ': 'o', 'ô': 'o', 'ó': 'o', 'ò': 'o', 'ỏ': 'o', 'õ': 'o', 'ọ': 'o',
  'Ư': 'W', 'Ú': 'U', 'Ù': 'U', 'Ủ': 'U', 'Ũ': 'U', 'Ụ': 'U',
  'ư': 'w', 'ú': 'u', 'ù': 'u', 'ủ': 'u', 'ũ': 'u', 'ụ': 'u',
  'Đ': 'D', 'đ': 'd',
};

// Define types for props and data structures
interface VariantAttribute {
  id: string;
  name: string;
}

interface Variant {
  id: string;
  Key: string;
  colorId: string;
  sizeId: string;
  price: string;
  stockQuantity: string;
  sku: string;
  imagePreview: string | null;
}

interface VariantTabProps {
  productName: string;
  productPrice: string;
  productImagePreview: string | null;
  register: UseFormRegister<any>;
  setValue: UseFormSetValue<any>;
  watch: UseFormWatch<any>;
  errors: FieldErrors<any>;
}

const debounce = (func: (value: string) => void, wait: number): ((value: string) => void) => {
  let timeout: NodeJS.Timeout;
  return (...args: [string]) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => func(...args), wait);
  };
};

const VariantTab: React.FC<VariantTabProps> = ({
  productName,
  productPrice,
  productImagePreview,
  register,
  setValue,
}) => {
  // State for search terms
  const [colorSearchTerm, setColorSearchTerm] = useState('');
  const [sizeSearchTerm, setSizeSearchTerm] = useState('');
  
  // State for available options (from API)
  const [availableColors, setAvailableColors] = useState<VariantAttribute[]>([]);
  const [availableSizes, setAvailableSizes] = useState<VariantAttribute[]>([]);
  
  // Selected states
  const [selectedColor, setSelectedColor] = useState<VariantAttribute | null>(null);
  const [selectedSizes, setSelectedSizes] = useState<VariantAttribute[]>([]);
  
  // Variants list
  const [variants, setVariants] = useState<Variant[]>([]);
  
  // Loading states
  const [loadingColors, setLoadingColors] = useState(false);
  const [loadingSizes, setLoadingSizes] = useState(false);

  const colorContainerRef = useRef<HTMLDivElement>(null);
  const sizeContainerRef = useRef<HTMLDivElement>(null);
  
  // Function to search for colors
  const searchColors = async (term: string) => {
    if (!term.trim()) {
      setAvailableColors([]);
      return;
    }
    
    setLoadingColors(true);
    try {
      // Replace with your actual API endpoint
      const response = await axiosConfig.get(`http://localhost:5113/api/Category/search?pattern=${encodeURIComponent(term)}`);
      if (response.status !== 200) throw new Error('Failed to fetch sizes');
      setAvailableColors(response.data);
    } catch (error) {
      console.error('Error fetching colors:', error);
    } finally {
      setLoadingColors(false);
    }
  };
  
  // Function to search for sizes
  const searchSizes = async (term: string) => {
    if (!term.trim()) {
      setAvailableSizes([]);
      return;
    }
    
    setLoadingSizes(true);
    try {
      // Replace with your actual API endpoint
      const response = await axiosConfig.get(`http://localhost:5113/api/Category/search?pattern=${encodeURIComponent(term)}`);
      if (response.status !== 200) throw new Error('Failed to fetch sizes');
      setAvailableSizes(response.data);
    } catch (error) {
      console.error('Error fetching sizes:', error);
      // In a real app, you might want to show an error message to the user
    } finally {
      setLoadingSizes(false);
    }
  };
  
  // Debounced search functions
  const debouncedSearchColors = useCallback(
    debounce((term: string) => {
      searchColors(term);
    }, 400),
    []
  );
  
  const debouncedSearchSizes = useCallback(
    debounce((term: string) => {
      searchSizes(term);
    }, 400),
    []
  );
  
  // Handle color search input change
  useEffect(() => {
    debouncedSearchColors(colorSearchTerm);
  }, [colorSearchTerm, debouncedSearchColors]);
  
  // Handle size search input change
  useEffect(() => {
    debouncedSearchSizes(sizeSearchTerm);
  }, [sizeSearchTerm, debouncedSearchSizes]);
  
  // Handle color selection
  const handleColorSelect = (color: VariantAttribute) => {
    setSelectedColor(color);
    // Clear previously selected sizes when changing color
    setSelectedSizes([]);
    
    // Clear search input and results for colors
    setColorSearchTerm('');
    setAvailableColors([]);
  };
  
  // Handle size selection
  const handleSizeSelect = (size: VariantAttribute) => {
    if (!selectedSizes.some(s => s.id === size.id)) {
      setSelectedSizes([...selectedSizes, size]);
    }
    
    // Clear search input and results for sizes
    setSizeSearchTerm('');
    setAvailableSizes([]);
  };
  
  // Handle removing the selected color
  const handleRemoveColor = () => {
    setSelectedColor(null);
    // Clear previously selected sizes when removing color
    setSelectedSizes([]);
  };
  
  // Handle removing a selected size
  const handleRemoveSize = (sizeId: string) => {
    setSelectedSizes(selectedSizes.filter(s => s.id !== sizeId));
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (colorContainerRef.current && 
          !colorContainerRef.current.contains(event.target as Node)) {
        setAvailableColors([]);
      }

      if (sizeContainerRef.current && 
        !sizeContainerRef.current.contains(event.target as Node)) {
      setAvailableSizes([]);
    }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);
  
  // Generate a formatted timestamp for SKU
  const generateTimestamp = () => {
    const now = new Date();
    return `${now.getFullYear()}${String(now.getMonth() + 1).padStart(2, '0')}${String(now.getDate()).padStart(2, '0')}${String(now.getHours()).padStart(2, '0')}${String(now.getMinutes()).padStart(2, '0')}`;
  };
  
  // Generate short name from product name (first letters of each word or first 3-4 letters)
  const generateShortProductName = () => {
    if (!productName) return 'PROD';
    
    const normalizeText = (text: string) =>
      text
        .split('')
        .map(char => vietnameseToEnglishMap[char] || char)
        .join('');
  
    // Option 1: First letters of each word
    const words = productName.split(' ');
    if (words.length > 1) {
      return words
        .map(word => normalizeText(word.charAt(0).toUpperCase()))
        .join('');
    }
  
    // Option 2: First 3-4 letters for single word product names
    return normalizeText(productName.substring(0, 3).toUpperCase());
  };
  
  // Generate variants based on selected color and sizes
  const generateVariants = () => {
    if (!selectedColor || selectedSizes.length === 0) {
      alert('Please select a color and at least one size to generate variants');
      return;
    }
    
    const shortName = generateShortProductName();
    const timestamp = generateTimestamp();
    const Key = `variant-${Date.now()}`
    
    const newVariants = selectedSizes.map((size, index) => {
      // Generate SKU: ShortNameOfProduct-Color-Size-DateTime
      const sku = `${shortName}-${selectedColor.name.toUpperCase()}-${size.name.toUpperCase()}-${timestamp}`;
      
      return {
        id: `${Key}-${index}`, // Generate a unique ID
        Key,
        colorId: selectedColor.id,
        sizeId: size.id,
        price: productPrice,
        stockQuantity: '0',
        sku,
        imagePreview: productImagePreview
      };
    });
    
    setVariants([...variants, ...newVariants]);
    
    // Update the form value for variants
    setValue('variants', [...variants, ...newVariants]);
  };
  
  // Update variant price
  const updateVariantPrice = (variantId: string, newPrice: string) => {
    const updatedVariants = variants.map(variant => 
      variant.id === variantId ? { ...variant, price: newPrice } : variant
    );
    setVariants(updatedVariants);
    setValue('variants', updatedVariants);
  };
  
  // Update variant stock
  const updateVariantStock = (variantId: string, newStock: string) => {
    const updatedVariants = variants.map(variant => 
      variant.id === variantId ? { ...variant, stockQuantity: newStock } : variant
    );
    setVariants(updatedVariants);
    setValue('variants', updatedVariants);
  };
  
  // Remove variant
  const removeVariant = (variantId: string) => {
    const updatedVariants = variants.filter(variant => variant.id !== variantId);
    setVariants(updatedVariants);
    setValue('variants', updatedVariants);
  };
  
  // Initialize variants in form data
  useEffect(() => {
    setValue('variants', variants);
  }, []);

  return (
    <div className="space-y-6">
      {/* Variant Creator Section */}
      <div className="bg-gray-50 p-4 rounded-md">
        <h3 className="text-lg font-medium text-gray-900 mb-4">Create Variants</h3>
        
        {/* Color & Size Creator */}
        <div className="flex flex-col md:flex-row md:space-x-4 space-y-4 md:space-y-0 mb-6">
          {/* Color Creator */}
          <div className="flex-1 relative" ref={colorContainerRef}>
            <label className="block text-sm font-medium text-gray-700 mb-1">Add Colors</label>

            {/* Colors suggestions dropdown */}
            {availableColors.length > 0 && (
              <div className="absolute z-10 top-full left-0 right-0 mb-1 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto">
                <ul className="py-1">
                  {availableColors.map((color) => (
                    <li 
                      key={color.id} 
                      className="px-3 py-2 hover:bg-blue-100 cursor-pointer text-gray-700"
                      onClick={() => handleColorSelect(color)}
                    >
                      {color.name}
                    </li>
                  ))}
                </ul>
              </div>
            )}

            {/* Input field */}
            <div className="flex">
              <input 
                type="text" 
                className="flex-1 border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Search for a color..."
                value={colorSearchTerm}
                onChange={(e) => setColorSearchTerm(e.target.value)}
                disabled={selectedColor !== null}
              />
            </div>
            {loadingColors && (
              <p className="text-sm text-gray-500 mt-1">Loading...</p>
            )}
          </div>
          
          {/* Size Creator */}
          <div className="flex-1 relative" ref={sizeContainerRef}>
            <label className="block text-sm font-medium text-gray-700 mb-1">Add Sizes</label>
            {/* Colors suggestions dropdown */}
            {availableSizes.length > 0 && (
              <div className="absolute z-10 top-full left-0 right-0 mb-1 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto">
                <ul className="py-1">
                  {availableSizes.map((size) => (
                    <li 
                      key={size.id} 
                      className="px-3 py-2 hover:bg-blue-100 cursor-pointer text-gray-700"
                      onClick={() => handleSizeSelect(size)}
                    >
                      {size.name}
                    </li>
                  ))}
                </ul>
              </div>
            )}
            <div className="flex">
              <input 
                type="text" 
                className="flex-1 border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Search for a size..."
                value={sizeSearchTerm}
                onChange={(e) => setSizeSearchTerm(e.target.value)}
              />
            </div>
            {loadingSizes && (
              <p className="text-sm text-gray-500 mt-1">Loading...</p>
            )}
          </div>
        </div>
        
        {/* Color Selection */}
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-2">Selected Color</label>
          
          {/* Selected Color */}
          {selectedColor && (
            <div className="mb-2">
              <div className="inline-flex items-center bg-blue-600 text-white px-3 py-1 rounded-full relative group">
                <span>{selectedColor.name}</span>
                <button
                  type="button"
                  onClick={() => handleRemoveColor()}
                  className="ml-2 w-4 h-4 rounded-full bg-white text-blue-600 flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity"
                >
                  ✕
                </button>
              </div>
            </div>
          )}
          
          {/* Available Colors */}
          {!selectedColor && (
            <div className="flex flex-wrap gap-2">
                <p className="text-sm text-gray-500">
                  {colorSearchTerm && !loadingColors
                    ? 'No colors found. Try a different search term.' 
                    : 'Search for colors above to select from'}
                </p>
            </div>
          )}
        </div>
        
        {/* Size Selection */}
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-2">Selected Sizes for this Color</label>
          
          {/* Selected Sizes */}
          {selectedSizes.length > 0 && (
            <div className="flex flex-wrap gap-2 mb-2">
              {selectedSizes.map((size) => (
                <div 
                  key={size.id}
                  className="inline-flex items-center bg-blue-600 text-white px-3 py-1 rounded-full relative group"
                >
                  <span>{size.name}</span>
                  <button
                    type="button"
                    onClick={() => handleRemoveSize(size.id)}
                    className="ml-2 w-4 h-4 rounded-full bg-white text-blue-600 flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity"
                  >
                    ✕
                  </button>
                </div>
              ))}
            </div>
          )}
          
          {/* Available Sizes */}
          {selectedColor && (
            <div className="flex flex-wrap gap-2">
              {availableSizes.length > 0 ? (
                availableSizes.map((size) => {
                  const isSelected = selectedSizes.some(s => s.id === size.id);
                  return (
                    !isSelected && (
                      <div 
                        key={size.id}
                        className="bg-gray-200 text-gray-700 hover:bg-gray-300 px-3 py-1 rounded-full cursor-pointer"
                        onClick={() => handleSizeSelect(size)}
                      >
                        {size.name}
                      </div>
                    )
                  );
                })
              ) : (
                <p className="text-sm text-gray-500">
                  {sizeSearchTerm && !loadingSizes
                    ? 'No sizes found. Try a different search term.' 
                    : 'Search for sizes above to select from'}
                </p>
              )}
            </div>
          )}
          
          {!selectedColor && (
            <p className="text-sm text-gray-500">
              Please select a color first
            </p>
          )}
        </div>
        
        {/* Color Image Upload - Using product image by default */}
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Product Image (Used for variants)
          </label>
          <div className="flex items-center space-x-4">
            <div className="w-16 h-16 bg-gray-100 rounded-md flex items-center justify-center overflow-hidden border border-gray-200">
              {productImagePreview ? (
                <img 
                  src={productImagePreview} 
                  alt="Product preview" 
                  className="w-full h-full object-cover" 
                />
              ) : (
                <span className="text-gray-400">No image</span>
              )}
            </div>
            <div className="flex-1">
              <p className="text-sm text-gray-500">
                Using the main product image for all variants
              </p>
            </div>
          </div>
        </div>
        
        {/* Generate Variants Button */}
        <div className="flex justify-end">
          <button
            type="button"
            className="px-4 py-2 rounded-md bg-blue-600 hover:bg-blue-700 text-white"
            onClick={generateVariants}
            disabled={!selectedColor || selectedSizes.length === 0}
          >
            Generate Variants
          </button>
        </div>
      </div>
      
      {/* Variants List */}
      <div>
        <h3 className="text-lg font-medium text-gray-900 mb-4">Product Variants</h3>
        
        {variants.length === 0 ? (
          <div className="bg-gray-50 p-4 rounded-md text-center text-gray-500">
            No variants created yet. Select a color and sizes above to generate variants.
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Variant</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Image</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Price</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Stock</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">SKU</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {variants.map((variant) => {
                  const colorName = availableColors.find(c => c.id === variant.colorId)?.name || 
                                   (selectedColor?.id === variant.colorId ? selectedColor.name : 'Unknown');
                  const sizeName = availableSizes.find(s => s.id === variant.sizeId)?.name || 
                                  selectedSizes.find(s => s.id === variant.sizeId)?.name || 'Unknown';
                  
                  return (
                    <tr key={variant.id}>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="text-sm font-medium text-gray-900">{colorName} / {sizeName}</div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="w-10 h-10 rounded-md overflow-hidden bg-gray-100 flex items-center justify-center">
                          {variant.imagePreview ? (
                            <img 
                              src={variant.imagePreview} 
                              alt={`${colorName} variant`} 
                              className="w-full h-full object-cover" 
                            />
                          ) : (
                            <span className="text-xs text-gray-400">No img</span>
                          )}
                        </div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <input
                          type="text"
                          value={variant.price}
                          onChange={(e) => updateVariantPrice(variant.id, e.target.value)}
                          className="w-20 border border-gray-300 rounded-md px-2 py-1 text-sm"
                        />
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <input
                          type="number"
                          min="0"
                          value={variant.stockQuantity}
                          onChange={(e) => updateVariantStock(variant.id, e.target.value)}
                          className="w-20 border border-gray-300 rounded-md px-2 py-1 text-sm"
                        />
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <input
                          type="text"
                          value={variant.sku}
                          readOnly
                          className="w-48 border border-gray-300 rounded-md px-2 py-1 text-sm bg-gray-50"
                        />
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <button
                          type="button"
                          onClick={() => removeVariant(variant.id)}
                          className="text-red-600 hover:text-red-900"
                        >
                          Remove
                        </button>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
        
        {/* Hidden input field to store variants data for form submission */}
        <input
          type="hidden"
          {...register('variants', { 
            validate: (value) => (value && value.length > 0) || 'At least one variant is required'
          })}
        />
      </div>
    </div>
  );
};

export default VariantTab;