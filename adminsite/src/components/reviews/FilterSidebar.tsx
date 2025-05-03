import { Filter, Calendar, Star, Search, X } from "lucide-react";
import React, { useState, useRef, useEffect } from "react";
import { Product } from '../../types/globalTypes';

// Define types for our filter props
export interface FilterSidebarProps {
  filtersOpen: boolean;
  setFiltersOpen: (open: boolean) => void;
  // Date range filters
  dateRange: { from: string; to: string };
  setDateRange: (range: { from: string; to: string }) => void;
  // Rating filter
  ratingFilter: number | null;
  setRatingFilter: (rating: number | null) => void;
  // Product filters
  products: Product[];
  selectedProducts: Product[];
  resetFilters: () => void;
}

const FilterSidebar: React.FC<FilterSidebarProps> = ({
  filtersOpen,
  setFiltersOpen,
  dateRange,
  setDateRange,
  ratingFilter,
  setRatingFilter,
  products,
  selectedProducts,
  resetFilters
}) => {
  const [productSearchTerm, setProductSearchTerm] = useState('');
  const [showProductDropdown, setShowProductDropdown] = useState(false);
  const searchRef = useRef<HTMLDivElement>(null);

  // Handle clicks outside of the product search dropdown
  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
        setShowProductDropdown(false);
      }
    }
    
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [searchRef]);

  const handleProductSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setProductSearchTerm(e.target.value);
    setShowProductDropdown(true);
  };

  const filteredProducts = products.filter(product => 
    product.name.toLowerCase().includes(productSearchTerm.toLowerCase())
  );

  return (
    <div className={`bg-blue-50 shadow ${filtersOpen ? 'w-64' : 'w-12'} transition-all duration-300 h-full flex flex-col`}>
      <div className="flex justify-between items-center p-4">
        <h3 className={`font-medium ${filtersOpen ? 'block' : 'hidden'}`}>Filters</h3>
        <button 
          onClick={() => setFiltersOpen(!filtersOpen)}
          className="p-1 rounded-md hover:bg-blue-100"
        >
          <Filter size={18} className="text-blue-500" />
        </button>
      </div>
      
      {filtersOpen && (
        <div className="space-y-6 p-4 flex-1 overflow-y-auto">
          {/* Product Filter */}
          <div ref={searchRef}>
            <h4 className="text-sm font-medium mb-2 flex items-center">
              <Search size={16} className="mr-2 text-blue-500" />
              Products
            </h4>
            <div className="flex items-center border border-gray-300 rounded-md bg-white px-3 py-2 focus-within:ring-2 focus-within:ring-blue-500">
              <input
                type="text"
                value={productSearchTerm}
                onChange={handleProductSearch}
                placeholder="Search products..."
                className="w-full outline-none text-sm bg-transparent"
                onFocus={() => setShowProductDropdown(true)}
              />
            </div>
            
            {/* Product dropdown */}
            {showProductDropdown && (
              <div className="absolute z-10 mt-1 w-56 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto">
                {filteredProducts.length === 0 ? (
                  <div className="p-3 text-gray-500 text-sm">No products found</div>
                ) : (
                  filteredProducts.map(product => (
                    <div
                      key={product.id}
                      className="p-2 hover:bg-gray-100 cursor-pointer text-sm"
                    >
                      {product.name}
                    </div>
                  ))
                )}
              </div>
            )}
            
            {/* Selected products */}
            {selectedProducts.length > 0 && (
              <div className="mt-2">
                <div className="flex items-center justify-between mb-1">
                  <span className="text-xs font-medium text-gray-700">Selected:</span>
                  <button
                    className="text-xs text-blue-600 hover:text-blue-800"
                  >
                    Clear
                  </button>
                </div>
                <div className="flex flex-wrap gap-1">
                  {selectedProducts.map(product => (
                    <div
                      key={product.id}
                      className="flex items-center bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs mb-1"
                    >
                      {product.name.length > 12 ? product.name.substring(0, 12) + '...' : product.name}
                      <button
                        className="ml-1 text-blue-600 hover:text-blue-800"
                      >
                        <X size={12} />
                      </button>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>

          {/* Date Range */}
          <div>
            <h4 className="text-sm font-medium mb-2 flex items-center">
              <Calendar size={16} className="mr-2 text-blue-500" />
              Date Created
            </h4>
            <div className="space-y-2">
              <div>
                <label className="block text-xs text-gray-500 mb-1">From</label>
                <input
                  type="date"
                  value={dateRange.from}
                  onChange={(e) => setDateRange({...dateRange, from: e.target.value})}
                  className="w-full p-2 border border-gray-300 rounded-md text-sm bg-white"
                />
              </div>
              <div>
                <label className="block text-xs text-gray-500 mb-1">To</label>
                <input
                  type="date"
                  value={dateRange.to}
                  onChange={(e) => setDateRange({...dateRange, to: e.target.value})}
                  className="w-full p-2 border border-gray-300 rounded-md text-sm bg-white"
                />
              </div>
            </div>
          </div>
          
          {/* Rating Filter */}
          <div>
            <h4 className="text-sm font-medium mb-2 flex items-center">
              <Star size={16} className="mr-2 text-blue-500" />
              Rating
            </h4>
            <div className="flex flex-wrap gap-1">
              <button
                onClick={() => setRatingFilter(null)}
                className={`px-2 py-1 rounded-md text-xs ${ratingFilter === null ? 'bg-blue-200 text-blue-800 font-medium' : 'bg-white text-gray-800'}`}
              >
                All
              </button>
              {[5, 4, 3, 2, 1].map(rating => (
                <button
                  key={rating}
                  onClick={() => setRatingFilter(rating)}
                  className={`px-2 py-1 rounded-md text-xs ${ratingFilter === rating ? 'bg-blue-200 text-blue-800 font-medium' : 'bg-white text-gray-800'}`}
                >
                  {rating}★
                </button>
              ))}
            </div>
          </div>
        </div>
      )}

      {/* Action Buttons at bottom */}
      {filtersOpen && (
        <div className="p-4 border-t border-blue-100 space-y-2">
          <button
            className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded"
          >
            Apply Filters
          </button>
          <button
            onClick={resetFilters}
            className="w-full bg-blue-100 hover:bg-blue-200 text-blue-800 font-medium py-2 px-4 rounded"
          >
            Reset Filters
          </button>
        </div>
      )}
    </div>
  );
};

export default FilterSidebar;