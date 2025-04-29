import { Search } from "lucide-react";
import { ReactNode } from "react";

interface PageHeaderProps {
  title?: string;
  searchPlaceholder?: string;
  onSearch?: (value: string) => void;
  searchValue?: string;
  buttonText?: string;
  onButtonClick?: () => void;
  buttonIcon?: ReactNode;
  showSearch?: boolean;
  showButton?: boolean;
}

/**
 * Reusable page header component that can be used across multiple pages
 */
const PageHeader = ({
  title = "Page Title",
  searchPlaceholder = "Search...",
  onSearch = () => {},
  searchValue = "",
  buttonText = "Action",
  onButtonClick = () => {},
  buttonIcon = null,
  showSearch = true,
  showButton = true
}: PageHeaderProps) => {
  return (
    <div className="p-6 w-full">
      <div className="flex items-center justify-between mb-4 w-full">
        <h2 className="text-xl font-semibold">{title}</h2>
        
        {showButton && (
          <button
            onClick={onButtonClick}
            className="flex items-center bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
          >
            {buttonIcon && <span className="mr-2">{buttonIcon}</span>}
            {buttonText}
          </button>
        )}
      </div>

      {/* Search bar */}
      {showSearch && (
        <div className="mb-4 w-full">
          <div className="relative w-full">
            <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
              <Search className="w-5 h-5 text-gray-400" />
            </div>
            <input
              type="text"
              className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
              placeholder={searchPlaceholder}
              value={searchValue}
              onChange={(e) => onSearch && onSearch(e.target.value)}
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default PageHeader;