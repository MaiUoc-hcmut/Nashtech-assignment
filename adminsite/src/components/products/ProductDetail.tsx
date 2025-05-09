import { useState, useEffect } from 'react';
import { Star, X } from 'lucide-react';
import axiosConfig from '../../redux/config/axios.config';

interface ProductDetailModalProps {
    productId: number;
    isOpen: boolean;
    onClose?: () => void;
}

interface ProductDetail {
    id: number;
    name: string;
    description: string;
    price: string;
    imageUrl: string;
    averageRating: number;
    totalOrders: number;
    createdAt: string;
    updatedAt: string;
    classifications: Array<{ id: number; name: string }>;
    variants: Array<{
        id: number;
        color?: { id: number; name: string };
        size?: { id: number; name: string };
        price: string;
        sku: string;
        stockQuantity: number;
        createdAt: string;
        updatedAt: string;
        imageUrl?: string;
    }>;
    reviews?: Array<{
        id: number;
        customerName: string;
        rating: number;
        text: string;
        createdAt: string;
    }>;
    orders?: Array<{
        id: number;
        amount: number;
        customerName: string;
        status: string;
        createdAt: string;
    }>;
}

const InitalProductDetail: ProductDetail = {
    id: 0,
    name: "",
    description: "",
    price: "",
    imageUrl: "",
    averageRating: 0,
    totalOrders: 0,
    createdAt: "",
    updatedAt: "",
    classifications: [],
    variants: [],
    reviews: [],
    orders: [],
};

// Status badge component
const StatusBadge: React.FC<{ status: string }> = ({ status }) => {
  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case 'completed':
        return 'bg-green-100 text-green-800';
      case 'processing':
        return 'bg-blue-100 text-blue-800';
      case 'shipped':
        return 'bg-purple-100 text-purple-800';
      case 'cancelled':
        return 'bg-red-100 text-red-800';
      case 'pending':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  return (
    <span className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(status)}`}>
      {status}
    </span>
  );
};

// ProductDetailModal component
const ProductDetailModal: React.FC<ProductDetailModalProps> = ({ productId, isOpen, onClose }) => {
  const [activeTab, setActiveTab] = useState('product');
  const [productData, setProductData] = useState<ProductDetail>(InitalProductDetail);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    const fetchProductData = async () => {
      try {
        setLoading(true);
        // Fetch product data with variants included
        const response = await axiosConfig.get(`http://localhost:5113/api/Product/${productId}?includeVariant=true`);
        
        if (response.status !== 200) {
          throw new Error('Failed to fetch product data');
        }
        const data = await response.data;
        
        // If the API doesn't return orders, we can mock them for demonstration
        if (!data.orders) {
          data.orders = [
            { id: 1001, amount: 299.99, customerName: "John Smith", status: "Completed", createdAt: "2025-04-15T10:30:00Z" },
            { id: 1002, amount: 59.99, customerName: "Emily Johnson", status: "Processing", createdAt: "2025-04-25T14:20:00Z" },
            { id: 1003, amount: 129.99, customerName: "Michael Brown", status: "Shipped", createdAt: "2025-04-20T09:15:00Z" },
            { id: 1004, amount: 89.99, customerName: "Sarah Davis", status: "Pending", createdAt: "2025-05-01T16:45:00Z" },
            { id: 1005, amount: 149.99, customerName: "David Wilson", status: "Cancelled", createdAt: "2025-04-18T11:20:00Z" }
          ];
        }
        
        setProductData(data);
        setLoading(false);
        console.log(data);
      } catch (error) {
        setError(error instanceof Error ? error.message : 'Unknown error occurred');
        setLoading(false);
      }
    };
    
    if (productId) {
      fetchProductData();
    }
  }, [productId]);

  // Format date function
  const formatDate = (dateString: string) => {
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  // Format currency
  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'VND'
    }).format(amount);
  };

  // Render stars for ratings
  const renderStars = (rating: number) => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      stars.push(
        <Star 
          key={i} 
          size={16} 
          className={i <= rating ? "text-yellow-500 fill-yellow-500" : "text-gray-300"} 
        />
      );
    }
    return stars;
  };

  const handleClose = () => {
    setError(null); 
    if (onClose) onClose(); 
  };

  if (!isOpen) return null;
  
  if (loading) {
    return (
      <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
        <div className="bg-white rounded-lg shadow-xl p-6">
          <p className="text-lg">Loading product details...</p>
        </div>
      </div>
    );
  }
  
  if (error) {
    return (
      <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
        <div className="bg-white rounded-lg shadow-xl p-6">
          <p className="text-lg text-red-600">Error: {error}</p>
          <button 
            onClick={handleClose}
            className="mt-4 px-4 py-2 bg-gray-200 hover:bg-gray-300 rounded text-gray-800"
          >
            Close
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
      <div className="bg-white rounded-lg shadow-xl w-4/5 h-4/5 flex flex-col">
        {/* Modal Header */}
        <div className="flex justify-between items-center p-4 border-b">
          <h2 className="text-xl font-semibold">Product Details</h2>
          <button onClick={handleClose} className="p-1 rounded-full hover:bg-gray-200">
            <X size={24} />
          </button>
        </div>
        
        {/* Tabs Navigation */}
        <div className="flex border-b">
          <button 
            className={`px-4 py-2 font-medium ${activeTab === 'product' ? 'border-b-2 border-blue-500 text-blue-600' : 'text-gray-600'}`}
            onClick={() => setActiveTab('product')}
          >
            Product
          </button>
          <button 
            className={`px-4 py-2 font-medium ${activeTab === 'variant' ? 'border-b-2 border-blue-500 text-blue-600' : 'text-gray-600'}`}
            onClick={() => setActiveTab('variant')}
          >
            Variants
          </button>
          <button 
            className={`px-4 py-2 font-medium ${activeTab === 'review' ? 'border-b-2 border-blue-500 text-blue-600' : 'text-gray-600'}`}
            onClick={() => setActiveTab('review')}
          >
            Reviews
          </button>
          <button 
            className={`px-4 py-2 font-medium ${activeTab === 'order' ? 'border-b-2 border-blue-500 text-blue-600' : 'text-gray-600'}`}
            onClick={() => setActiveTab('order')}
          >
            Orders
          </button>
        </div>
        
        {/* Tab Content */}
        <div className="flex-1 overflow-auto p-4">
          {/* Product Tab */}
          {activeTab === 'product' && (
            <div className="flex flex-col md:flex-row gap-6">
              <div className="md:w-1/3">
                <img 
                  src={productData.imageUrl || "/api/placeholder/600/400"} 
                  alt={productData.name} 
                  className="w-full h-auto rounded-lg object-cover"
                />
              </div>
              <div className="md:w-2/3">
                <h1 className="text-2xl font-bold mb-2">{productData.name}</h1>
                <div className="text-xl font-semibold text-blue-600 mb-4">
                  ${parseFloat(productData.price)}
                </div>
                <div className="mb-4">
                  <div className="flex items-center mb-2">
                    <div className="flex items-center">
                      {[...Array(5)].map((_, i) => (
                        <Star 
                          key={i} 
                          size={16} 
                          className={i < Math.round(productData.averageRating) ? "text-yellow-500 fill-yellow-500" : "text-gray-300"} 
                        />
                      ))}
                    </div>
                    <span className="ml-2 text-sm text-gray-600">
                      {productData.averageRating} rating
                    </span>
                    <span className="mx-2 text-gray-300">|</span>
                    <span className="text-sm text-gray-600">
                      {productData.totalOrders} orders
                    </span>
                  </div>
                </div>
                <div className="mb-4">
                  <h3 className="text-lg font-semibold mb-2">Description</h3>
                  <div 
                    className="p-3 min-h-16 text-sm prose"
                    dangerouslySetInnerHTML={{ 
                      __html: productData.description
                    }}
                  ></div>
                </div>
                <div className="mb-4">
                  <h3 className="text-lg font-semibold mb-2">Classification</h3>
                  <div className="flex flex-wrap gap-2">
                    {productData.classifications && productData.classifications.map(classification => (
                      <span key={classification.id} className="px-2 py-1 bg-gray-100 rounded-md text-sm text-gray-700">
                        {classification.name}
                      </span>
                    ))}
                  </div>
                </div>
                <div className="text-sm text-gray-500">
                  <p>Created: {formatDate(productData.createdAt)}</p>
                  <p>Last Updated: {formatDate(productData.updatedAt)}</p>
                </div>
              </div>
            </div>
          )}
          
          {/* Variant Tab */}
          {activeTab === 'variant' && (
            <div>
              <h3 className="text-lg font-semibold mb-4">Product Variants</h3>
              <div className="overflow-x-auto">
                {productData.variants && productData.variants.length > 0 ? (
                  <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                      <tr>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Image</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Color</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Size</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Price</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">SKU</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Stock</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Created</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Updated</th>
                      </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                      {productData.variants.map((variant) => (
                        <tr key={variant.id}>
                          <td className="px-6 py-4 whitespace-nowrap">
                            <img 
                              src={variant.imageUrl || productData.imageUrl} 
                              alt={`Variant ${variant.id}`} 
                              className="h-12 w-12 object-cover rounded" 
                            />
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{variant.id}</td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                            {variant.color ? variant.color.name : '-'}
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                            {variant.size ? variant.size.name : '-'}
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                            ${parseFloat(variant.price)}
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{variant.sku}</td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                            <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${variant.stockQuantity > 10 ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'}`}>
                              {variant.stockQuantity}
                            </span>
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                            {formatDate(variant.createdAt)}
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                            {formatDate(variant.updatedAt)}
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                ) : (
                  <div className="text-center py-6 text-gray-500">No variants available for this product.</div>
                )}
              </div>
            </div>
          )}
          
          {/* Review Tab */}
          {activeTab === 'review' && (
            <div>
              <div className="flex justify-between items-center mb-4">
                <h3 className="text-lg font-semibold">Customer Reviews</h3>
                <span className="text-sm text-gray-600">
                  {productData.reviews ? productData.reviews.length : 0} reviews
                </span>
              </div>
              <div className="space-y-4">
                {productData.reviews && productData.reviews.length > 0 ? (
                  productData.reviews.map((review) => (
                    <div key={review.id} className="border rounded-lg p-4">
                      <div className="flex justify-between items-start mb-2">
                        <div>
                          <p className="font-medium">{review.customerName}</p>
                          <div className="flex items-center my-1">
                            {renderStars(review.rating)}
                          </div>
                        </div>
                        <span className="text-sm text-gray-500">{formatDate(review.createdAt)}</span>
                      </div>
                      <p className="text-gray-700">{review.text}</p>
                    </div>
                  ))
                ) : (
                  <div className="text-center py-6 text-gray-500">No reviews available for this product.</div>
                )}
              </div>
            </div>
          )}
          
          {/* Order Tab */}
          {activeTab === 'order' && (
            <div>
              <div className="flex justify-between items-center mb-4">
                <h3 className="text-lg font-semibold">Orders</h3>
                <span className="text-sm text-gray-600">
                  {productData.orders ? productData.orders.length : 0} orders
                </span>
              </div>
              <div className="overflow-x-auto">
                {productData.orders && productData.orders.length > 0 ? (
                  <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                      <tr>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Order ID</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Customer</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Amount</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Date</th>
                      </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                      {productData.orders.map((order) => (
                        <tr key={order.id}>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{order.id}</td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{order.customerName}</td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                            {formatCurrency(order.amount)}
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap">
                            <StatusBadge status={order.status} />
                          </td>
                          <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                            {formatDate(order.createdAt)}
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                ) : (
                  <div className="text-center py-6 text-gray-500">No orders available for this product.</div>
                )}
              </div>
            </div>
          )}
        </div>
        
        {/* Modal Footer */}
        <div className="border-t p-4 flex justify-end">
          <button 
            onClick={handleClose}
            className="px-4 py-2 bg-gray-200 hover:bg-gray-300 rounded text-gray-800"
          >
            Close
          </button>
        </div>
      </div>
    </div>
  );
}

export default ProductDetailModal;