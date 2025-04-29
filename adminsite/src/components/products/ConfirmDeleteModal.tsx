import React from 'react';
import { useForm } from 'react-hook-form';

// Interface for form data
interface FormValues {
  password: string;
}

// Interface for component props
interface ConfirmDeleteProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (password: string) => Promise<void>;
  error?: string | null;
}

const ConfirmDelete: React.FC<ConfirmDeleteProps> = ({ isOpen, onClose, onSubmit, error }) => {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<FormValues>({
    defaultValues: {
      password: '',
    },
  });

  // Reset form when modal closes
  React.useEffect(() => {
    if (!isOpen) {
      reset();
    }
  }, [isOpen, reset]);

  // Handle form submission
  const handleFormSubmit = handleSubmit(async (data: FormValues): Promise<void> => {
    await onSubmit(data.password);
  });

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg shadow-lg overflow-hidden w-full max-w-md">
        <h3 className="text-lg font-semibold p-4 border-b text-red-500">Confirm Deletion</h3>
        <form onSubmit={handleFormSubmit} className="p-6 space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Enter Password to Confirm</label>
            <input
              type="password"
              className={`mt-1 block w-full border ${
                errors.password ? 'border-red-500' : 'border-gray-300'
              } rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500`}
              {...register('password', {
                required: 'Password is required',
                minLength: { value: 6, message: 'Password must be at least 6 characters' },
              })}
            />
            {errors.password && (
              <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>
            )}
            {error && <p className="text-red-500 text-sm mt-1">{error}</p>}
          </div>
          <div className="flex justify-end space-x-2">
            <button
              type="button"
              onClick={onClose}
              disabled={isSubmitting}
              className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-100 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Delete
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ConfirmDelete;