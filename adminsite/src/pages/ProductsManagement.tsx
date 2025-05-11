import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../redux/hooks/redux';
import { fetchProducts, addProduct, deleteProduct } from '../redux/features/products/productsSlice';
import { Plus, ArrowUpDown, Star, RefreshCw } from "lucide-react";
// import { Link } from 'react-router-dom';
import ProductModal from "../components/products/AddProductModal";
import ConfirmDelete from "../components/products/ConfirmDeleteModal";
import ProductDetailModal from '../components/products/ProductDetail';
import FilterSidebar, { FilterRange } from "../components/products/FilterSidebar";
import PageHeader from "../components/commons/PageHeader";
import PageFooter from "../components/commons/PageFooter";
import { AddProduct } from '../types/globalTypes';

const ProductPage: React.FC = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDetailModalOpen, setIsDetailModalOpen] = useState(false);
  const [isConfirmDeleteModalOpen, setIsConfirmDeleteModalOpen] = useState(false);
  const [idDelete, setIdDelete] = useState(0);
  const [isPending, setIsPending] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState<'CreatedAt' | 'Price' | 'Rating'>('CreatedAt');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc'); // Default to newest first
  const [filtersOpen, setFiltersOpen] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [productIdOpen, setProductIdOpen] = useState(0);
  
  // Filter states
  const startDate = new Date('2025-01-01');
  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate());
  const formatDateToInput = (date: Date) => date.toISOString().split('T')[0];
  const [dateRange, setDateRange] = useState<{ from: string; to: string }>({
    from: formatDateToInput(startDate),
    to: formatDateToInput(tomorrow), 
  });
  const [priceRange, setPriceRange] = useState<FilterRange>({
    min: 0,
    max: 1000000
  });
  const [ratingRange, setRatingRange] = useState<FilterRange>({
    min: 0,
    max: 5
  });
  
  const dispatch = useAppDispatch();
  const { totalProducts, products, status, error } = useAppSelector((state) => state.products);

  useEffect(() => {
    if (status === "idle") {
      dispatch(fetchProducts({
      currentPage,
      sortBy: "CreatedAt",
      isAsc: false,
      minPrice: priceRange.min,
      maxPrice: priceRange.max,
      minRating: ratingRange.min,
      maxRating: ratingRange.max,
      startDate: dateRange.from,
      endDate: dateRange.to,
    }));
    }

    if (status === "loading") {
      setIsPending(true);
    }

    if (status !== "loading") {
      setIsPending(false);
    }
  }, [dispatch, status, products]);

  useEffect(() => {
    dispatch(fetchProducts({
      currentPage,
      sortBy,
      isAsc: sortOrder === 'asc' ? true : false,
      minPrice: priceRange.min,
      maxPrice: priceRange.max,
      minRating: ratingRange.min,
      maxRating: ratingRange.max,
      startDate: dateRange.from,
      endDate: dateRange.to,
    }));
  }, [currentPage]);

  const handleApplyFilter = async () => {
    dispatch(fetchProducts({
      currentPage: 1,
      sortBy: "CreatedAt",
      isAsc: false,
      minPrice: priceRange.min,
      maxPrice: priceRange.max,
      minRating: ratingRange.min,
      maxRating: ratingRange.max,
      startDate: dateRange.from,
      endDate: dateRange.to,
    }));
  }

  const handleSubmit = async (data: AddProduct) => {
    await dispatch(addProduct(data)).unwrap();
    setIsModalOpen(false);
  };

  const handleDelete = async (password: string) => {
    const data = {
      id: idDelete,
      password
    }
    await dispatch(deleteProduct(data)).unwrap();
    setIsConfirmDeleteModalOpen(false);
  };

  const handleOpenDeleteModal = (id: number) => {
    setIsConfirmDeleteModalOpen(true);
    setIdDelete(id);
  };

  const handleSort = (column: 'CreatedAt' | 'Price' | 'Rating') => {
    if (sortBy === column) {
      setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
    } else {
      setSortBy(column);
      setSortOrder(column === 'CreatedAt' ? 'desc' : 'asc');
    }
    dispatch(fetchProducts({
      currentPage: 1,
      sortBy,
      isAsc: sortOrder == 'asc' ? true : false,
      minPrice: priceRange.min,
      maxPrice: priceRange.max,
      minRating: ratingRange.min,
      maxRating: ratingRange.max,
      startDate: dateRange.from,
      endDate: dateRange.to,
    }));
  };

  const resetFilters = () => {
    setDateRange({ from: startDate.toLocaleDateString('en-US'), to: tomorrow.toLocaleDateString('en-US') });
    setPriceRange({ min: 0, max: 1000000 });
    setRatingRange({ min: 0, max: 5 });
    setSearchTerm('');
  };

  const handleSearch = (value: string) => {
    setSearchTerm(value);
    setCurrentPage(1);
  };

  const handleRefreshReviews = () => {
    // UI only - actual logic to be implemented by the user
    setIsPending(true);
    setTimeout(() => {
      setIsPending(false);
    }, 500); // Simulate loading for UI feedback
  };

  const SortIcon = ({ column }: { column: 'CreatedAt' | 'Price' | 'Rating' }) => {
    if (sortBy !== column) return <ArrowUpDown size={16} className="ml-1 text-gray-400" />;
    return sortOrder === 'asc' ? 
      <ArrowUpDown size={16} className="ml-1 text-blue-500" /> : 
      <ArrowUpDown size={16} className="ml-1 text-blue-500 transform rotate-180" />;
  };

  const totalPages = Math.ceil(totalProducts / 10);

  return (
    <div className="flex h-full">
      {/* Filter Sidebar Component */}
      <FilterSidebar 
        filtersOpen={filtersOpen}
        setFiltersOpen={setFiltersOpen}
        dateRange={dateRange}
        setDateRange={setDateRange}
        priceRange={priceRange}
        setPriceRange={setPriceRange}
        ratingRange={ratingRange}
        setRatingRange={setRatingRange}
        applyFilter={handleApplyFilter}
        resetFilters={resetFilters}
      />
      
      {/* Main Content */}
      <div className="flex flex-col h-full w-full bg-white rounded-lg shadow overflow-hidden">
        {/* Using the reusable PageHeader component */}
        <PageHeader
          title="Product Management"
          searchPlaceholder="Search products..."
          searchValue={searchTerm}
          onSearch={handleSearch}
          buttonText="Add Product"
          onButtonClick={() => setIsModalOpen(true)}
          buttonIcon={<Plus size={20} />}
        />

        {error && <p className="text-red-500 mx-6 mb-4">{error}</p>}

        {/* Table section with scroll */}
        <div className="flex-1 overflow-auto">
          {isPending ? (
            <div className="flex justify-center items-center h-32">
              <div className="text-blue-500 flex items-center">
                <RefreshCw size={20} className="animate-spin mr-2" />
                Loading...
              </div>
            </div>
          ) : (
            <div className="flex-1 overflow-auto px-6 pb-6 w-full">
              <table className="w-full table-auto">
                <thead className="sticky top-0 bg-white w-full">
                  <tr className="bg-gray-100 w-full">
                    <th className="px-4 py-2 text-left">Image</th>
                    <th className="px-4 py-2 text-left">Name</th>
                    <th 
                      className="px-4 py-2 text-left cursor-pointer"
                      onClick={() => handleSort('CreatedAt')}
                    >
                      <div className="flex items-center">
                        Created Date
                        <SortIcon column="CreatedAt" />
                      </div>
                    </th>
                    <th 
                      className="px-4 py-2 text-left cursor-pointer"
                      onClick={() => handleSort('Price')}
                    >
                      <div className="flex items-center">
                        Price
                        <SortIcon column="Price" />
                      </div>
                    </th>
                    <th 
                      className="px-4 py-2 text-left cursor-pointer"
                      onClick={() => handleSort('Rating')}
                    >
                      <div className="flex items-center">
                        Rating
                        <SortIcon column="Rating" />
                      </div>
                    </th>
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
                  {totalProducts === 0 ? (
                    <tr>
                      <td colSpan={6} className="text-center py-4">
                        {searchTerm || dateRange.from || dateRange.to || priceRange.min > 0 || priceRange.max < 1000000 || ratingRange.min > 0 || ratingRange.max < 5 ? 
                          'No products match your filters' : 'No products found'}
                      </td>
                    </tr>
                  ) : (
                    products.map((product) => (
                      <tr key={product.id} className="border-b hover:bg-gray-50">
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
                        <td className="px-4 py-2">{product.createdAt ? new Date(product.createdAt).toLocaleDateString() : '2025-04-28'}</td>
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
                        <td className="px-4 py-2 space-x-2">
                          <button 
                            onClick={() => {
                              setProductIdOpen(product.id);
                              setIsDetailModalOpen(true);
                            }}
                            className="border border-blue-500 text-blue-500 bg-transparent hover:bg-blue-500 hover:text-white px-3 py-1 rounded text-sm"
                          >
                            View
                          </button>
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
          )}
        </div>
        

        {/* Using the reusable PageFooter component */}
        <PageFooter
          currentPage={currentPage}
          totalPages={totalPages}
          totalItems={totalProducts}
          itemsPerPage={10}
          currentItemCount={products.length}
          onPageChange={setCurrentPage}
          itemLabel="products"
        />

        {/* Modals */}
        <ProductModal 
          isOpen={isModalOpen}
          isPending={isPending}
          onClose={() => setIsModalOpen(false)}
          onSubmit={handleSubmit}
          error={error}
        />

        {/* Product detail modal */}
        <ProductDetailModal 
          isOpen={isDetailModalOpen}
          onClose={() => {
            setIsDetailModalOpen(false);
            setProductIdOpen(0);
          }}
          productId={productIdOpen}
        />

        <ConfirmDelete 
          isOpen={isConfirmDeleteModalOpen}
          onClose={() => setIsConfirmDeleteModalOpen(false)}
          onSubmit={handleDelete}
          error={error}
        />
      </div>
    </div>
  );
};

export default ProductPage;