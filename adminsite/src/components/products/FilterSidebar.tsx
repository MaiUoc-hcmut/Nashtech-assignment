import { Filter, Calendar, DollarSign, Star } from "lucide-react";
import RangeFilter from './RangeFilter';

// Define types for our filter props
export interface FilterRange {
  min: number;
  max: number;
}

export interface FilterSidebarProps {
  filtersOpen: boolean;
  setFiltersOpen: (open: boolean) => void;
  // Date range filters
  dateRange: { from: string; to: string };
  setDateRange: (range: { from: string; to: string }) => void;
  // Price range filters
  priceRange: FilterRange;
  setPriceRange: (range: FilterRange) => void;
  // Rating range filters
  ratingRange: FilterRange;
  setRatingRange: (range: FilterRange) => void;
  // Apply filter
  applyFilter: () => void;
  // Reset filters
  resetFilters: () => void;
}

const FilterSidebar: React.FC<FilterSidebarProps> = ({
  filtersOpen,
  setFiltersOpen,
  dateRange,
  setDateRange,
  priceRange,
  setPriceRange,
  ratingRange,
  setRatingRange,
  applyFilter,
  resetFilters
}) => {
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
                  className="w-full p-2 border border-gray-300 rounded-md text-sm"
                />
              </div>
              <div>
                <label className="block text-xs text-gray-500 mb-1">To</label>
                <input
                  type="date"
                  value={dateRange.to}
                  onChange={(e) => setDateRange({...dateRange, to: e.target.value})}
                  className="w-full p-2 border border-gray-300 rounded-md text-sm"
                />
              </div>
            </div>
          </div>
          
          {/* Price Range Filter */}
          <RangeFilter
            icon={<DollarSign size={16} className="mr-2 text-blue-500" />}
            title="Price Range"
            minValue={0}
            maxValue={1000000}
            step={10000}
            currentRange={priceRange}
            setRange={setPriceRange}
            formatValue={(value) => `${value.toLocaleString()}đ`}
          />
          
          {/* Rating Range Filter */}
          <RangeFilter
            icon={<Star size={16} className="mr-2 text-blue-500" />}
            title="Rating Range"
            minValue={0}
            maxValue={5}
            step={0.5}
            currentRange={ratingRange}
            setRange={setRatingRange}
            formatValue={(value) => value.toFixed(1)}
          />
        </div>
      )}

      {/* Reset Filters Button at bottom */}
      {filtersOpen && (
        <div className="border-t border-blue-100 p-4">
          <div className="space-y-2">
            <button
              onClick={resetFilters}
              className="w-full bg-blue-100 hover:bg-blue-200 text-blue-800 font-medium py-2 px-4 rounded"
            >
              Reset Filters
            </button>
            <button
              onClick={applyFilter}
              className="w-full bg-blue-500 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded"
            >
              Apply Filters
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default FilterSidebar;