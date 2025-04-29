import StarRating from "./StarRating";

const ProductCard: React.FC<{
  product: any;
  onDelete: () => void;
}> = ({ product, onDelete }) => {
  return (
    <div className="group bg-white rounded-lg shadow-md overflow-hidden flex flex-col transition-all duration-300 hover:shadow-lg border border-transparent hover:border-blue-100">
      {/* Product Image */}
      <div className="relative overflow-hidden h-52 bg-gray-50">
        <img
          src={product.imageUrl}
          alt={product.name}
          className="h-full w-full object-contain transition-transform duration-300 group-hover:scale-105"
        />
        
        {/* Quick Action Buttons - Delete Button */}
        <div className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity duration-200">
          <button
            onClick={(e) => {
              e.stopPropagation();
              onDelete();
            }}
            className="bg-white bg-opacity-90 text-red-500 p-2 rounded-full shadow-md hover:bg-red-500 hover:text-white transition-colors"
            aria-label="Delete product"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" className="w-4 h-4">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>
      
      {/* Product Info */}
      <div className="p-4 flex flex-col flex-grow">
        <h3 className="font-medium text-gray-800 mb-1 line-clamp-2 h-12" title={product.name}>
          {product.name}
        </h3>
        
        {/* Rating */}
        <div className="mb-2 flex items-center">
          <StarRating rating={product.rating} />
          <span className="text-xs text-gray-500 ml-1">({product.rating})</span>
        </div>
        
        {/* Price */}
        <div className="mt-auto pt-2">
          <span className="text-lg font-bold text-gray-900">{product.price}đ</span>
        </div>
      </div>
    </div>
  );
};

export default ProductCard;