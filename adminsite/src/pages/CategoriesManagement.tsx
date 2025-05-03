import { useState, useEffect, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../redux/hooks/redux';
import { fetchCategories, updateCategory, deleteCategory, addCategory } from '../redux/features/categories/categoriesSlice';
import { ClassificationNCateGoryNParent } from '../types/dashboardTypes';
import { Trash2, Edit, Plus, RefreshCw } from 'lucide-react';
import { useForm } from 'react-hook-form';
import PageHeader from '../components/commons/PageHeader';
import PageFooter from '../components/commons/PageFooter';
import axiosConfig from '../redux/config/axios.config';

const CategoriesManagement: React.FC = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<ClassificationNCateGoryNParent | null>(null);
  const [isPending, setIsPending] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [parentSearchTerm, setParentSearchTerm] = useState('');
  const [pCategories, setPCategories] = useState<ClassificationNCateGoryNParent[]>([]);
  const [selectedParent, setSelectedParent] = useState<ClassificationNCateGoryNParent | null>(null);

  const dispatch = useAppDispatch();
  const { categories, status, error } = useAppSelector((state) => state.categories);
  const { parentCategories } = useAppSelector((state) => state.parentCategories);
  const parentCategoryContainerRef = useRef<HTMLDivElement>(null);

  const {
    register,
    handleSubmit,
    reset,
    setValue,
    formState: { errors }
  } = useForm<{ name: string; parent: number, description: string }>({
    defaultValues: {
      name: '',
      parent: 0,
      description: '',
    }
  });

  // Fetch categories on mount
  useEffect(() => {
    if (status == "idle") {
      dispatch(fetchCategories());
    }

    if (status == "loading") {
      setIsPending(true);
    }

    if (status != "loading") {
      setIsPending(false);
    }
  }, [dispatch, status, categories]);

  // Handle searching for parent categories
  const searchParentCategories = async (query: string) => {
    if (!query.trim()) {
      setPCategories([]);
      return;
    }
    if (parentCategories.length > 0) {
      const filteredParentCategories = parentCategories.filter((category) => {
        return category.name.toLowerCase().includes(query.toLowerCase());
      });
      setPCategories(filteredParentCategories);
    } else {
      const filteredParentCategories = await axiosConfig.get(`http://localhost:5113/api/ParentCategory/search?pattern=${query}`);
      if (filteredParentCategories.status !== 200) throw new Error('Failed to fetch categories');
      setPCategories(filteredParentCategories.data);
    }
    
  };

  // Handle clicking outside the parent category container
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (parentCategoryContainerRef.current && 
          !parentCategoryContainerRef.current.contains(event.target as Node)) {
        setPCategories([]);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Handle parent category selection
  const handleSelectParent = (parentCategory: ClassificationNCateGoryNParent) => {
    setSelectedParent(parentCategory);
    setValue('parent', parentCategory.id, { shouldValidate: true });
    setParentSearchTerm('');
    setPCategories([]);
  };

  // Debounce utility function
  const debounce = (func: (value: string) => void, wait: number): ((value: string) => void) => {
    let timeout: NodeJS.Timeout;
    return (value: string) => {
      clearTimeout(timeout);
      timeout = setTimeout(() => func(value), wait);
    };
  };

  // Debounced search for parent categories
  const debouncedSearch = debounce((value: string) => {
    searchParentCategories(value);
  }, 300);

  // Handle parent search input change
  useEffect(() => {
    debouncedSearch(parentSearchTerm);
  }, [parentSearchTerm]);

  const onSubmit = handleSubmit(async (data) => {
    if (editingCategory) {
      // Update existing category
      await dispatch(updateCategory({
        id: editingCategory.id,
        ...data
      })).unwrap();
    } 
    else {
      // Add new category
      await dispatch(addCategory({
        ...data
      })).unwrap();
    }
    
    setIsModalOpen(false);
    reset();
    setSelectedParent(null);
  });

  const handleDelete = async (id: number) => {
    await dispatch(deleteCategory(id)).unwrap();
  }

  const handleRefreshReviews = () => {
    // UI only - actual logic to be implemented by the user
    setIsPending(true);
    setTimeout(() => {
      setIsPending(false);
    }, 500); // Simulate loading for UI feedback
  };

  const openModal = (category?: ClassificationNCateGoryNParent) => {
    setEditingCategory(category || null);
    // if (category) {
    //   reset({
    //     name: category.name,
    //     parent: category.parentCategory?.id || 0,
    //     description: category.description
    //   });
    //   setSelectedParent(category.parentCategory || null);
    // } else {
      reset({
        name: '',
        parent: 0,
        description: ''
      });
      setSelectedParent(null);
    // }
    setIsModalOpen(true);
  };

  const handleSearch = (value: string) => {
    setSearchTerm(value);
    setCurrentPage(1); // Reset to first page when searching
  };

  const itemsPerPage = 10;
  const totalPages = Math.ceil(categories.length / itemsPerPage);
  const currentItems = categories.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  return (
    <div className="bg-white rounded-lg shadow p-6">
      <PageHeader
        title="Categories Management"
        searchPlaceholder="Search categories..."
        searchValue={searchTerm}
        onSearch={handleSearch}
        buttonText="Add Category"
        onButtonClick={() => openModal()}
        buttonIcon={<Plus size={20} />}
      />

      {error && <p className="text-red-500 mb-4">{error}</p>}
      {isPending && <p className="text-blue-500 mb-4">Loading Categories...</p>}

      <div className="flex-1 overflow-auto px-6 pb-6 w-full">
        <table className="w-full table-auto">
          <thead>
            <tr className="bg-gray-100">
              <th className="px-4 py-2 text-left">ID</th>
              <th className="px-4 py-2 text-left">Name</th>
              <th className="px-4 py-2 text-left">Description</th>
              <th className="px-4 py-2 text-left">Parent</th>
              <th className="px-4 py-2 text-left">
                {/* Refresh Button */}
                <RefreshCw 
                  size={18} 
                  className={`mr-2 ${isPending ? 'animate-spin' : ''} hover:cursor-pointer`}
                  onClick={handleRefreshReviews}
                />
              </th>
            </tr>
          </thead>
          <tbody>
            {categories.length === 0 ? (
              <tr>
                <td colSpan={4} className="text-center py-4">No Categories found</td>
              </tr>
            ) : (
              categories.map((category) => (
                <tr key={category.id} className="border-b">
                  <td className="px-4 py-2">{category.id}</td>
                  <td className="px-4 py-2">{category.name}</td>
                  <td className="px-4 py-2">{category.description}</td>
                  <td className="px-4 py-2">{category.parentCategory?.name || 'No Parent'}</td>
                  <td className="px-4 py-2 flex space-x-2">
                    <button
                      onClick={() => openModal(category)}
                      className="text-blue-600 hover:text-blue-800"
                    >
                      <Edit size={20} />
                    </button>
                    <button
                      onClick={() => handleDelete(category.id)}
                      className="text-red-600 hover:text-red-800"
                    >
                      <Trash2 size={20} />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <PageFooter
        currentPage={currentPage}
        totalPages={totalPages}
        totalItems={categories.length}
        itemsPerPage={itemsPerPage}
        currentItemCount={currentItems.length}
        onPageChange={setCurrentPage}
        itemLabel="products"
      />

      {isModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white rounded-lg p-6 w-full max-w-md">
            <h3 className="text-lg font-semibold mb-4">
              {editingCategory ? 'Edit Category' : 'Add Category'}
            </h3>
            <form onSubmit={onSubmit} className="space-y-4">
              {/* Name field */}
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

              {/* Parent category field with search */}
              <div className="relative" ref={parentCategoryContainerRef}>
                <label className="block text-sm font-medium text-gray-700">Parent Category</label>
                
                {/* Hidden input to store the selected parent ID */}
                <input
                  type="hidden"
                  {...register("parent")}
                />
                
                {/* Parent category suggestions dropdown */}
                {pCategories.length > 0 && (
                  <div className="absolute z-10 top-full left-0 right-0 mb-1 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto">
                    <ul className="py-1">
                      {pCategories.map((pCategory) => (
                        <li 
                          key={pCategory.id} 
                          className="px-3 py-2 hover:bg-blue-100 cursor-pointer text-gray-700"
                          onClick={() => handleSelectParent(pCategory)}
                        >
                          {pCategory.name}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}
                
                {/* Input field with selected parent display */}
                <div className={`mt-1 flex items-center border ${errors.parent ? 'border-red-500' : 'border-gray-300'} rounded-md px-3 py-2 focus-within:ring-2 focus-within:ring-blue-500`}>
                  {selectedParent ? (
                    <div className="flex items-center bg-blue-100 text-blue-800 rounded-md px-2 py-1 mr-2 text-sm">
                      {selectedParent.name}
                      <button
                        type="button"
                        className="ml-1 text-blue-500 hover:text-blue-700 focus:outline-none"
                        onClick={() => {
                          setSelectedParent(null);
                          setValue('parent', 0);
                        }}
                      >
                        &times;
                      </button>
                    </div>
                  ) : null}
                  
                  <input
                    type="text"
                    value={parentSearchTerm}
                    onChange={(e) => setParentSearchTerm(e.target.value)}
                    className="flex-grow border-none focus:outline-none focus:ring-0"
                    placeholder={selectedParent ? "" : "Search parent category..."}
                  />
                </div>
              </div>
                
              {/* Description field */}
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
              {error && <p className="text-red-500 text-sm">{error}</p>}
              <div className="flex justify-end space-x-2">
                <button
                  type="button"
                  onClick={() => {
                    setIsModalOpen(false);
                    setSelectedParent(null);
                  }}
                  className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-100"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                >
                  {editingCategory ? 'Update' : 'Add'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default CategoriesManagement;