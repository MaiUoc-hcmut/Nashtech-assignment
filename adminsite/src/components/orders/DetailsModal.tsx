import { useEffect } from 'react';
import { X, Package, Calendar, MapPin, User, DollarSign, SmartphoneNfc } from 'lucide-react';


interface Customer {
  name: string;
  email?: string;
  phoneNumber?: string;
}

interface Product {
  name: string;
  price: number;
  imageUrl: string;
}

interface VariantOrder {
  id: number;
  sku: string;
  price: number;
  imageUrl: string;
  product: Product;
  quantity: number;
}

export interface OrderDetail {
  id: number;
  amount: number;
  address: string;
  status: string;
  createdAt: string;
  email?: string;
  phoneNumber?: string;
  customerName?: string;
  paymentMethod?: string;
  shippingMethod?: string;
  variants: VariantOrder[];
  customer: Customer;
  notes?: string;
}

interface OrderDetailModalProps {
  isOpen: boolean;
  onClose: () => void;
  orderDetail: OrderDetail | null;
  loading?: boolean;
  error?: string | null;
}

const OrderDetailModal: React.FC<OrderDetailModalProps> = ({ 
  isOpen, 
  onClose, 
  orderDetail,
  loading = false,
  error = null
}) => {

  // Close modal on escape key
  useEffect(() => {
    const handleEscape = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        onClose();
      }
    };

    if (isOpen) {
      document.addEventListener('keydown', handleEscape);
      document.body.style.overflow = 'hidden'; // Prevent background scroll
    }

    return () => {
      document.removeEventListener('keydown', handleEscape);
      document.body.style.overflow = 'unset';
    };
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  const formatDate = (dateString: string) => {
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'VND',
    }).format(amount);
  };

  const handleBackdropClick = (e: React.MouseEvent) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };

  return (
    <div 
      className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4"
      onClick={handleBackdropClick}
    >
      <div className="bg-white rounded-lg shadow-xl max-w-4xl w-full max-h-[90vh] overflow-hidden">
        {/* Modal Header */}
        <div className="flex items-center justify-between p-6 border-b bg-gray-50">
          <h2 className="text-2xl font-bold text-gray-800">
            Order Details {orderDetail ? `#${orderDetail.id}` : ''}
          </h2>
          <button
            onClick={onClose}
            className="p-2 hover:bg-gray-200 rounded-full transition-colors"
            aria-label="Close modal"
          >
            <X size={24} className="text-gray-500" />
          </button>
        </div>

        {/* Modal Content */}
        <div className="p-6 overflow-y-auto max-h-[calc(90vh-120px)]">
          {loading ? (
            <div className="flex justify-center items-center h-32">
              <div className="text-blue-500 flex items-center">
                <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-blue-500 mr-2"></div>
                Loading order details...
              </div>
            </div>
          ) : error ? (
            <div className="text-center py-8">
              <p className="text-red-500 text-lg">{error}</p>
              <button
                onClick={onClose}
                className="mt-4 px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600 transition-colors"
              >
                Close
              </button>
            </div>
          ) : orderDetail ? (
            <div className="space-y-6">
              {/* Contact Information and Customer Information - Same Line */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-4">
                  <h3 className="text-lg font-semibold text-gray-800 flex items-center">
                    <SmartphoneNfc size={20} className="mr-2 text-blue-500" />
                    Contact Information
                  </h3>
                  <div className="bg-gray-50 p-4 rounded-lg space-y-2">
                    <p><span className="font-medium">Name:</span> {orderDetail.customerName}</p>
                    {orderDetail.email && <p><span className="font-medium">Email:</span> {orderDetail.email}</p>}
                    {orderDetail.phoneNumber && <p><span className="font-medium">Phone:</span> {orderDetail.phoneNumber}</p>}
                  </div>
                </div>

                <div className="space-y-4">
                  <h3 className="text-lg font-semibold text-gray-800 flex items-center">
                    <User size={20} className="mr-2 text-blue-500" />
                    Customer Information
                  </h3>
                  <div className="bg-gray-50 p-4 rounded-lg space-y-2">
                    <p><span className="font-medium">Name:</span> {orderDetail.customer.name}</p>
                    {orderDetail.customer.email && <p><span className="font-medium">Email:</span> {orderDetail.customer.email}</p>}
                    {orderDetail.customer.phoneNumber && <p><span className="font-medium">Phone:</span> {orderDetail.customer.phoneNumber}</p>}
                  </div>
                </div>
              </div>

              {/* Order Summary and Shipping Information - Same Line */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-4">
                  <h3 className="text-lg font-semibold text-gray-800 flex items-center">
                    <DollarSign size={20} className="mr-2 text-green-500" />
                    Order Summary
                  </h3>
                  <div className="bg-gray-50 p-4 rounded-lg space-y-2">
                    <p><span className="font-medium">Status:</span> 
                      <span className="ml-2 px-2 py-1 rounded-full text-xs font-medium bg-green-100 text-green-800">
                        {orderDetail.status}
                      </span>
                    </p>
                    <p><span className="font-medium">Total Amount:</span> {formatCurrency(orderDetail.amount)}</p>
                    {orderDetail.paymentMethod && <p><span className="font-medium">Payment:</span> {orderDetail.paymentMethod}</p>}
                  </div>
                </div>

                <div className="space-y-4">
                  <h3 className="text-lg font-semibold text-gray-800 flex items-center">
                    <MapPin size={20} className="mr-2 text-red-500" />
                    Shipping Information
                  </h3>
                  <div className="bg-gray-50 p-4 rounded-lg space-y-2">
                    <p><span className="font-medium">Address:</span> {orderDetail.address}</p>
                    {orderDetail.shippingMethod && <p><span className="font-medium">Shipping Method:</span> {orderDetail.shippingMethod}</p>}
                    <p className="flex items-center">
                      <Calendar size={16} className="mr-2" />
                      <span className="font-medium">Order Date:</span> 
                      <span className="ml-1">{formatDate(orderDetail.createdAt)}</span>
                    </p>
                  </div>
                </div>
              </div>

              {/* Products */}
              <div className="space-y-4">
                <h3 className="text-lg font-semibold text-gray-800 flex items-center">
                  <Package size={20} className="mr-2 text-purple-500" />
                  Products ({orderDetail.variants.length} items)
                </h3>
                <div className="bg-gray-50 p-4 rounded-lg">
                  <div className="space-y-4">
                    {orderDetail.variants.map((variant) => (
                      <div key={variant.id} className="flex items-center space-x-4 bg-white p-4 rounded-lg shadow-sm">
                        <img
                          src={variant.imageUrl ? variant.imageUrl : variant.product.imageUrl}
                          alt={variant.product.name}
                          className="w-16 h-16 object-cover rounded-lg"
                          onError={(e) => {
                            const target = e.target as HTMLImageElement;
                            target.src = 'https://via.placeholder.com/100x100?text=No+Image';
                          }}
                        />
                        <div className="flex-1">
                          <h4 className="font-medium text-gray-800">{variant.product.name}</h4>
                          <p className="text-sm text-gray-600">
                            Unit Price: {formatCurrency(variant.price)}
                          </p>
                        </div>
                        <div className="text-center">
                          <p className="text-sm text-gray-600">Quantity</p>
                          <p className="font-medium">{variant.quantity}</p>
                        </div>
                        <div className="text-right">
                          <p className="text-sm text-gray-600">Total</p>
                          <p className="font-medium text-lg">{formatCurrency(variant.price * variant.quantity)}</p>
                        </div>
                      </div>
                    ))}
                  </div>
                  
                  {/* Order Total */}
                  <div className="border-t mt-4 pt-4">
                    <div className="flex justify-between items-center">
                      <span className="text-lg font-semibold">Order Total:</span>
                      <span className="text-xl font-bold text-green-600">
                        {formatCurrency(orderDetail.amount)}
                      </span>
                    </div>
                  </div>
                </div>
              </div>

              {/* Notes */}
              {orderDetail.notes && (
                <div className="space-y-4">
                  <h3 className="text-lg font-semibold text-gray-800">Notes</h3>
                  <div className="bg-yellow-50 p-4 rounded-lg border-l-4 border-yellow-400">
                    <p className="text-gray-700">{orderDetail.notes}</p>
                  </div>
                </div>
              )}
            </div>
          ) : null}
        </div>
      </div>
    </div>
  );
};

export default OrderDetailModal;