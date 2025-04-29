import { useState } from 'react';
// import { useAppSelector } from '../redux/hooks/redux';
import { Search, Plus, ArrowUpDown, Star } from "lucide-react";
import ProductModal from "./AddProductModal";
import ConfirmDelete from "./ConfirmDeleteModal";

interface ProductListProps {
  products: any[];
  sortBy: 'updatedAt' | 'price' | 'rating';
  sortOrder: 'asc' | 'desc';
  handleSort: (column: 'updatedAt' | 'price' | 'rating') => void;
  searchTerm: string;
  setSearchTerm: (term: string) => void;
  isPending: boolean;
  error: string | null;
  handleSubmit: (data: any) => void;
  handleDelete: (password: string) => void;
  handleOpenDeleteModal: (id: number) => void;
}

const ProductList: React.FC<ProductListProps> = ({
  products,
  sortBy,
  sortOrder,
  handleSort,
  searchTerm,
  setSearchTerm,
  isPending,
  error,
  handleSubmit,
  handleDelete,
  handleOpenDeleteModal
}) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isConfirmDeleteModalOpen, setIsConfirmDeleteModalOpen] = useState(false);

  const SortIcon = ({ column }: { column: 'updatedAt' | 'price' | 'rating' }) => {
    if (sortBy !== column) return <ArrowUpDown size={16} className="ml-1 text-gray-400" />;
    return sortOrder === 'asc' ? 
      <ArrowUpDown size={16} className="ml-1 text-blue-500" /> : 
      <ArrowUpDown size={16} className="ml-1 text-blue-500 transform rotate-180" />;
  };

  return (
    <div className="flex flex-col h-full w-full bg-white rounded-lg shadow overflow-hidden">
      {/* Header section */}
      <div className="p-6 w-full">
        <div className="flex items-center justify-between mb-4 w-full">
          <h2 className="text-xl font-semibold">Product Management</h2>
          <button
            onClick={() => setIsModalOpen(true)}
            className="flex items-center bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
          >
            <Plus size={20} className="mr-2" />
            Add Product
          </button>
        </div>

        {/* Search bar */}
        <div className="mb-4 w-full">
          <div className="relative w-full">
            <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
              <Search className="w-5 h-5 text-gray-400" />
            </div>
            <input
              type="text"
              className="block w-full pl-10 pr nessa-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
              placeholder="Search products..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>

        {error && <p className="text-red-500 mb-4">{error}</p>}
        {isPending && <p className="text-blue-500 mb-4">Loading Products...</p>}
      </div>

      {/* Table section with scroll */}
      <div className="flex-1 overflow-auto px-6 pb-6 w-full">
        <table className="w-full table-auto">
          <thead className="sticky top-0 bg-white w-full">
            <tr className="bg-gray-100 w-full">
              <th className="px-4 py-2 text-left">Image</th>
              <th className="px-4 py-2 text-left">Name</th>
              <th 
                className="px-4 py-2 text-left cursor-pointer"
                onClick={() => handleSort('updatedAt')}
              >
                <div className="flex items-center">
                  Updated Date
                  <SortIcon column="updatedAt" />
                </div>
              </th>
              <th 
                className="px-4 py-2 text-left cursor-pointer"
                onClick={() => handleSort('price')}
              >
                <div className="flex items-center">
                  Price
                  <SortIcon column="price" />
                </div>
              </th>
              <th 
                className="px-4 py-2 text-left cursor-pointer"
                onClick={() => handleSort('rating')}
              >
                <div className="flex items-center">
                  Rating
                  <SortIcon column="rating" />
                </div>
              </th>
              <th className="px-4 py-2 text-left"></th>
            </tr>
          </thead>
          <tbody>
            {products.length === 0 ? (
              <tr>
                <td colSpan={6} className="text-center py-4">
                  {searchTerm ? 'No products match your filters' : 'No products found'}
                </td>
              </tr>
            ) : (
              products.map((product) => (
                <tr key={product.id} className="border-b hover:bg-gray-50 cursor-pointer">
                  <td className="px-4 py-2">
                    <div className="h-16 w-16 bg-gray-100 rounded">
                      <img
                        src={product.imageUrl}
                        alt={product.name}
                        className="h-full w-full object-contain"
                      />
                    </div>
                  </td>
                  <td className="px-4 py-2 font-medium">{product.name}</td>
                  <td className="px-4 py-2">{product.updatedAt ? new Date(product.updatedAt).toLocaleDateString() : '2025-04-28'}</td>
                  <td className="px-4 py-2 font-bold">{product.price}đ</td>
                  <td className="px-4 py-2">
                    <div className="flex items-center">
                      <div className="flex text-yellow-400 mr-1">
                        {[...Array(5)].map((_, i) => (
                          <Star
                            key={i}
                            size={16}
                            fill={i < Math.round(product.rating) ? "currentColor" : "none"}
                            className={i < Math.round(product.rating) ? "text-yellow-400" : "text-gray-300"}
                          />
                        ))}
                      </div>
                      <span className="text-sm text-gray-500">({product.rating > 0 ? product.rating.toFixed(1) : 0})</span>
                    </div>
                  </td>
                  <td className="px-4 py-2">
                    <button
                      onClick={() => handleOpenDeleteModal(product.id)}
                      className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded text-sm"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Pagination Footer */}
      <div className="mt-auto px-4 py-3 bg-gray-50 border-t border-gray-200">
        <div className="flex items-center justify-between">
          <div className="flex items-center text-sm text-gray-500">
            <span>Showing 1-{products.length} of {products.length} products</span>
          </div>
          <div className="flex items-center space-x-2">
            <button 
              className="px-3 py-1 bg-white border border-gray-300 rounded-md text-sm hover:bg-gray-50 disabled:opacity-50"
              disabled={true}
            >
              Previous
            </button>
            <button className="px-3 py-1 bg-blue-600 text-white rounded-md text-sm hover:bg-blue-700">1</button>
            <button className="px-3 py-1 bg-white border border-gray-300 rounded-md text-sm hover:bg-gray-50">2</button>
            <button className="px-3 py-1 bg-white border border-gray-300 rounded-md text-sm hover:bg-gray-50">3</button>
            <button className="px-3 py-1 bg-white border border-gray-300 rounded-md text-sm hover:bg-gray-50">
              Next
            </button>
          </div>
        </div>
      </div>

      {/* Modals */}
      <ProductModal 
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleSubmit}
        error={error}
      />

      <ConfirmDelete 
        isOpen={isConfirmDeleteModalOpen}
        onClose={() => setIsConfirmDeleteModalOpen(false)}
        onSubmit={handleDelete}
        error={error}
      />
    </div>
  );
};

export default ProductList;