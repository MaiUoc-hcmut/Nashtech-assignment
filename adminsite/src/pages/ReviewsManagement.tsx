import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../redux/hooks/redux';
import { fetchReviews } from '../redux/features/reviews/reviewsSlice';
import { Eye, Star, ChevronUp, ChevronDown, RefreshCw } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import PageHeader from '../components/commons/PageHeader';
import PageFooter from '../components/commons/PageFooter';
import FilterSidebar from '../components/reviews/FilterSidebar';

const ReviewsManagement: React.FC = () => {
  const [isPending, setIsPending] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedProducts, setSelectedProducts] = useState([]);
  const [sortField, setSortField] = useState<'rating' | 'createdAt'>('createdAt');
  const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('desc');
  const [ratingFilter, setRatingFilter] = useState<number | null>(null);
  const [dateRange, setDateRange] = useState({ from: '', to: '' });
  const [filtersOpen, setFiltersOpen] = useState(true);
  const [activeFilters, setActiveFilters] = useState({
    products: [],
    rating: null,
    dateFrom: '',
    dateTo: ''
  });
  
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const { reviews, status: reviewsStatus } = useAppSelector((state) => state.reviews);
  const { products, status: productsStatus } = useAppSelector((state) => state.products);

  // Fetch reviews and products on mount
  useEffect(() => {
    if (reviewsStatus === "idle") {
      dispatch(fetchReviews());
    }

    if (reviewsStatus === "loading") {
      setIsPending(true);
    } else {
      setIsPending(false);
    }
  }, [dispatch, reviewsStatus, productsStatus]);

  // Format date for display
  const formatDate = (dateString: string) => {
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  const handleViewReview = (reviewId: number) => {
    navigate(`/reviews/${reviewId}`);
  };

  const handleSearch = (value: string) => {
    setSearchTerm(value);
    setCurrentPage(1);
  };

  const handleRefreshReviews = async () => {
    setIsPending(true);
    await dispatch(fetchReviews()).unwrap();
    setIsPending(false);
  };

  const handleSortChange = (field: 'rating' | 'createdAt') => {
    if (sortField === field) {
      setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
    } else {
      setSortField(field);
      setSortDirection('desc');
    }
  };

  const resetFilters = () => {
    setSelectedProducts([]);
    setRatingFilter(null);
    setDateRange({ from: '', to: '' });
    setActiveFilters({
      products: [],
      rating: null,
      dateFrom: '',
      dateTo: ''
    });
    setCurrentPage(1);
    dispatch(fetchReviews());
  };

  // Filter and sort reviews
  const filteredAndSortedReviews = reviews
    .filter(review => {
      const matchesSearch = 
        review.customer.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        review.text.toLowerCase().includes(searchTerm.toLowerCase()) ||
        review.product.name.toLowerCase().includes(searchTerm.toLowerCase());
      
      const matchesRating = activeFilters.rating === null || review.rating === activeFilters.rating;

      const reviewDate = new Date(review.createdAt);
      const matchesStartDate = !activeFilters.dateFrom || reviewDate >= new Date(activeFilters.dateFrom);
      const matchesEndDate = !activeFilters.dateTo || reviewDate <= new Date(activeFilters.dateTo);
      
      return matchesSearch && matchesRating && matchesStartDate && matchesEndDate;
    })
    .sort((a, b) => {
      if (sortField === 'rating') {
        return sortDirection === 'asc' 
          ? a.rating - b.rating
          : b.rating - a.rating;
      } else {
        return sortDirection === 'asc'
          ? new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
          : new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
      }
    });

  const itemsPerPage = 10;
  const totalPages = Math.ceil(filteredAndSortedReviews.length / itemsPerPage);
  const currentItems = filteredAndSortedReviews.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  // Render star rating
  const renderStars = (rating: number) => {
    return (
      <div className="flex">
        {Array.from({ length: 5 }).map((_, index) => (
          <Star
            key={index}
            size={16}
            fill={index < rating ? "#FFD700" : "none"}
            stroke={index < rating ? "#FFD700" : "#CBD5E0"}
          />
        ))}
      </div>
    );
  };

  // Render sort indicator
  const renderSortIndicator = (field: 'rating' | 'createdAt') => {
    if (sortField !== field) return null;
    
    return sortDirection === 'asc' 
      ? <ChevronUp size={16} className="ml-1" /> 
      : <ChevronDown size={16} className="ml-1" />;
  };

  return (
    <div className="flex h-screen bg-gray-50">
      {/* Filter Sidebar Component */}
      <FilterSidebar
        filtersOpen={filtersOpen}
        setFiltersOpen={setFiltersOpen}
        dateRange={dateRange}
        setDateRange={setDateRange}
        ratingFilter={ratingFilter}
        setRatingFilter={setRatingFilter}
        products={products}
        selectedProducts={selectedProducts}
        resetFilters={resetFilters}
      />

      {/* Main Content */}
      <div className="flex-1 overflow-auto">
        <div className="bg-white rounded-lg shadow-md h-full flex flex-col">
          <div className="border-b border-gray-200">
            <PageHeader
              title="Reviews Management"
              searchPlaceholder="Search reviews..."
              searchValue={searchTerm}
              onSearch={handleSearch}
              showButton={false}
              showSearch={false}
            />
          </div>

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
                    <tr className='bg-gray-100 w-full'>
                      <th className="px-4 py-3 text-left">Customer</th>
                      <th className="px-4 py-3 text-left">Product</th>
                      <th className="px-4 py-3 text-left">
                        <div 
                          className="flex items-center cursor-pointer"
                          onClick={() => handleSortChange('rating')}
                        >
                          Rating {renderSortIndicator('rating')}
                        </div>
                      </th>
                      <th className="px-4 py-3 text-left">
                        <div 
                          className="flex items-center cursor-pointer"
                          onClick={() => handleSortChange('createdAt')}
                        >
                          Date {renderSortIndicator('createdAt')}
                        </div>
                      </th>
                      <th className="px-4 py-3 text-left">Comment</th>
                      <th className="px-4 py-3 text-center">
                        {/* Refresh Button */}
                        <RefreshCw 
                          size={18} 
                          className="hover:cursor-pointer text-blue-500 hover:text-blue-700"
                          onClick={handleRefreshReviews}
                        />
                      </th>
                    </tr>
                  </thead>
                  <tbody className="bg-white divide-y divide-gray-200">
                    {filteredAndSortedReviews.length === 0 ? (
                      <tr>
                        <td colSpan={6} className="text-center py-8 text-gray-500">No reviews found</td>
                      </tr>
                    ) : (
                      currentItems.map((review) => (
                        <tr key={review.id} className="hover:bg-gray-50">
                          <td className="px-4 py-3 whitespace-nowrap">{review.customer.name}</td>
                          <td className="px-4 py-3 whitespace-nowrap">{review.product.name}</td>
                          <td className="px-4 py-3 whitespace-nowrap">{renderStars(review.rating)}</td>
                          <td className="px-4 py-3 whitespace-nowrap">{formatDate(review.createdAt)}</td>
                          <td className="px-4 py-3">
                            <div className="max-w-xs truncate">{review.text}</div>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap text-center">
                            <button
                              onClick={() => handleViewReview(review.id)}
                              className="text-blue-600 hover:text-blue-800 p-1 rounded-full hover:bg-blue-50"
                              title="View Review Details"
                            >
                              <Eye size={18} />
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

          <div className="p-4 border-t border-gray-200">
            <PageFooter
              currentPage={currentPage}
              totalPages={totalPages}
              totalItems={filteredAndSortedReviews.length}
              itemsPerPage={itemsPerPage}
              currentItemCount={currentItems.length}
              onPageChange={setCurrentPage}
              itemLabel="reviews"
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default ReviewsManagement;