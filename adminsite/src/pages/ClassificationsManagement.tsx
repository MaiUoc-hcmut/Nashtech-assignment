import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../redux/hooks/redux';
import { fetchClassifications, updateClassification, deleteClassification, addClassification } from '../redux/features/classifications/classificationsSlice';
import { ClassificationNCateGoryNParent } from '../types/dashboardTypes';
import { Trash2, Edit, Plus, RefreshCw } from 'lucide-react';
import { useForm } from 'react-hook-form';
import PageHeader from '../components/commons/PageHeader';
import PageFooter from '../components/commons/PageFooter';

const ClassificationsManagement: React.FC = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingClassification, setEditingClassification] = useState<ClassificationNCateGoryNParent | null>(null);
  const [isPending, setIsPending] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);

  
  const dispatch = useAppDispatch();
  const { classifications, status, error } = useAppSelector((state) => state.classifications);

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

  // Fetch classifications on mount
  useEffect(() => {
    if (status == "idle") {
      dispatch(fetchClassifications());
    }

    if (status == "loading") {
      setIsPending(true);
    }

    if (status != "loading") {
      setIsPending(false);
    }
  }, [dispatch, status, classifications]);

  const onSubmit = handleSubmit(async (data) => {
    if (editingClassification) {
      // Update existing classification
      await dispatch(updateClassification({
        id: editingClassification.id,
        ...data
      })).unwrap();
    } 
    else {
      // Add new classification
      await dispatch(addClassification({
        ...data
      })).unwrap();
    }
    
    setIsModalOpen(false);
    reset();
  });

  const handleDelete = async (id: number) => {
    await dispatch(deleteClassification(id)).unwrap();
  }

  const handleRefreshReviews = () => {
    // UI only - actual logic to be implemented by the user
    setIsPending(true);
    setTimeout(() => {
      setIsPending(false);
    }, 500); // Simulate loading for UI feedback
  };

  const openModal = (classification?: ClassificationNCateGoryNParent) => {
    setEditingClassification(classification || null);
    if (classification) {
      reset({
        name: classification.name,
        description: classification.description
      });
    } else {
      reset({
        name: '',
        description: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleSearch = (value: string) => {
    setSearchTerm(value);
    setCurrentPage(1);
  };

  const itemsPerPage = 10;
  const totalPages = Math.ceil(classifications.length / itemsPerPage);
  const currentItems = classifications.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  return (
    <div className="bg-white rounded-lg shadow p-6">
      <PageHeader
        title="Classifications Management"
        searchPlaceholder="Search clssifications..."
        searchValue={searchTerm}
        onSearch={handleSearch}
        buttonText="Add Classification"
        onButtonClick={() => openModal()}
        buttonIcon={<Plus size={20} />}
      />

      {error && <p className="text-red-500 mb-4">{error}</p>}
      {isPending && <p className="text-blue-500 mb-4">Loading Classifications...</p>}

      <div className="flex-1 overflow-auto px-6 pb-6 w-full">
        <table className="w-full table-auto">
          <thead>
            <tr className="bg-gray-100">
              <th className="px-4 py-2 text-left">ID</th>
              <th className="px-4 py-2 text-left">Name</th>
              <th className="px-4 py-2 text-left">Description</th>
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
            {classifications.length === 0 ? (
              <tr>
                <td colSpan={4} className="text-center py-4">No classifications found</td>
              </tr>
            ) : (
              classifications.map((classification) => (
                <tr key={classification.id} className="border-b">
                  <td className="px-4 py-2">{classification.id}</td>
                  <td className="px-4 py-2">{classification.name}</td>
                  <td className="px-4 py-2">{classification.description}</td>
                  <td className="px-4 py-2 flex space-x-2">
                    <button
                      onClick={() => openModal(classification)}
                      className="text-blue-600 hover:text-blue-800"
                    >
                      <Edit size={20} />
                    </button>
                    <button
                    onClick={() => handleDelete(classification.id)}
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
        totalItems={classifications.length}
        itemsPerPage={itemsPerPage}
        currentItemCount={currentItems.length}
        onPageChange={setCurrentPage}
        itemLabel="products"
      />

      {isModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white rounded-lg p-6 w-full max-w-md">
            <h3 className="text-lg font-semibold mb-4">
              {editingClassification ? 'Edit Classification' : 'Add Classification'}
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
                  {editingClassification ? 'Update' : 'Add'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default ClassificationsManagement;