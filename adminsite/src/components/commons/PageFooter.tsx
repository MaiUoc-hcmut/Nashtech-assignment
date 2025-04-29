interface PageFooterProps {
  currentPage?: number;
  totalPages?: number;
  totalItems?: number;
  itemsPerPage?: number;
  currentItemCount?: number;
  onPageChange?: (page: number) => void;
  itemLabel?: string;
  showPagination?: boolean;
  showItemCount?: boolean;
}

/**
 * Reusable page footer component with pagination that can be used across multiple pages
 */
const PageFooter = ({
  currentPage = 1,
  totalPages = 1,
  totalItems = 0,
  itemsPerPage = 10,
  currentItemCount = 0,
  onPageChange = () => {},
  itemLabel = "items",
  showPagination = true,
  showItemCount = true
}: PageFooterProps) => {
  // Calculate the start and end item numbers
  const startItem = totalItems === 0 ? 0 : (currentPage - 1) * itemsPerPage + 1;
  const endItem = Math.min(startItem + currentItemCount - 1, totalItems);
  
  // Generate array of page numbers to display
  const getPageNumbers = () => {
    const pageNumbers = [];
    const maxPagesToShow = 3; // Show at most 3 page numbers
    
    let startPage = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(totalPages, startPage + maxPagesToShow - 1);
    
    // Adjust if we're near the end
    if (endPage - startPage + 1 < maxPagesToShow) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pageNumbers.push(i);
    }
    
    return pageNumbers;
  };

  if (totalItems == 0) return null;

  return (
    <div className="mt-auto px-4 py-3 bg-gray-50 border-t border-gray-200">
      <div className="flex items-center justify-between">
        {showItemCount && (
          <div className="flex items-center text-sm text-gray-500">
            <span>
              {`Showing ${startItem}-${endItem} of ${totalItems} ${itemLabel}`}
            </span>
          </div>
        )}
        
        {showPagination && totalPages > 1 && (
          <div className="flex items-center space-x-2">
            <button 
              className="px-3 py-1 bg-white border border-gray-300 rounded-md text-sm hover:bg-gray-50 disabled:opacity-50"
              disabled={currentPage === 1}
              onClick={() => onPageChange && onPageChange(currentPage - 1)}
            >
              Previous
            </button>
            
            {getPageNumbers().map(pageNum => (
              <button 
                key={pageNum}
                className={`px-3 py-1 rounded-md text-sm ${
                  pageNum === currentPage 
                    ? 'bg-blue-600 text-white hover:bg-blue-700' 
                    : 'bg-white border border-gray-300 hover:bg-gray-50'
                }`}
                onClick={() => onPageChange && onPageChange(pageNum)}
              >
                {pageNum}
              </button>
            ))}
            
            <button 
              className="px-3 py-1 bg-white border border-gray-300 rounded-md text-sm hover:bg-gray-50 disabled:opacity-50"
              disabled={currentPage === totalPages}
              onClick={() => onPageChange && onPageChange(currentPage + 1)}
            >
              Next
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default PageFooter;