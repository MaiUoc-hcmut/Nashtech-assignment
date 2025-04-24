import { AlertCircle } from 'lucide-react';
import { LowStockItem } from '../types/dashboardTypes';

interface LowStockItemsProps {
  items: LowStockItem[];
}

const LowStockItems: React.FC<LowStockItemsProps> = ({ items }) => {
  return (
    <div className="bg-white rounded-lg shadow p-6">
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-xl font-semibold">Low Stock Alert</h2>
        <span className="bg-red-100 text-red-800 text-xs font-medium px-2.5 py-0.5 rounded-full flex items-center">
          <AlertCircle size={12} className="mr-1" /> {items.length} items
        </span>
      </div>
      <div className="overflow-y-auto max-h-56">
        {items.map(item => (
          <div key={item.id} className="border-b border-gray-100 py-3">
            <p className="font-medium">{item.name}</p>
            <div className="flex justify-between text-sm text-gray-500 mt-1">
              <span>SKU: {item.sku}</span>
              <span className="text-red-600 font-medium">Stock: {item.stock}</span>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default LowStockItems;