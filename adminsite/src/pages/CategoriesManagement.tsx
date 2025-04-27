import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../redux/hooks/redux';
import { fetchCategories, updateCategory, deleteCategory, addCategory } from '../redux/features/categories/categoriesSlice';
import { ClassificationNCateGoryNParent } from '../types/dashboardTypes';
import { Trash2, Edit, Plus } from 'lucide-react';
import { useForm } from 'react-hook-form';

const CategoriesManagement: React.FC = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<ClassificationNCateGoryNParent | null>(null);
  const [isPending, setIsPending] = useState(false);

  const dispatch = useAppDispatch();
  const { categories, status, error } = useAppSelector((state) => state.categories);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors }
  } = useForm<{ name: string; description: string }>({
    defaultValues: {
      name: '',
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
  });

  const handleDelete = async (id: number) => {
    await dispatch(deleteCategory(id)).unwrap();
  }

  const openModal = (category?: ClassificationNCateGoryNParent) => {
    setEditingCategory(category || null);
    if (category) {
      reset({
        name: category.name,
        description: category.description
      });
    } else {
      reset({
        name: '',
        description: ''
      });
    }
    setIsModalOpen(true);
  };

  return (
    <div className="bg-white rounded-lg shadow p-6">
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-xl font-semibold">Categories Management</h2>
        <button
          onClick={() => openModal()}
          className="flex items-center bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
        >
          <Plus size={20} className="mr-2" />
          Add Category
        </button>
      </div>

      {error && <p className="text-red-500 mb-4">{error}</p>}
      {isPending && <p className="text-blue-500 mb-4">Loading Categories...</p>}

      <table className="w-full table-auto">
        <thead>
          <tr className="bg-gray-100">
            <th className="px-4 py-2 text-left">ID</th>
            <th className="px-4 py-2 text-left">Name</th>
            <th className="px-4 py-2 text-left">Description</th>
            <th className="px-4 py-2 text-left">Actions</th>
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

      {isModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white rounded-lg p-6 w-full max-w-md">
            <h3 className="text-lg font-semibold mb-4">
              {editingCategory ? 'Edit Category' : 'Add Category'}
            </h3>
            <form onSubmit={onSubmit} className="space-y-4">
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
              {error && <p className="text-red-500 text-sm">{error}</p>}
              <div className="flex justify-end space-x-2">
                <button
                  type="button"
                  onClick={() => setIsModalOpen(false)}
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