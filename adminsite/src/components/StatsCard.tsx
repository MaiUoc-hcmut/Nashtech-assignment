import { ShoppingBag, Package, AlertCircle, Users, DollarSign } from 'lucide-react';
import { Stats } from '../types/dashboardTypes';

interface StatsCardProps {
  title: string;
  value: number | string;
  icon: string;
}

const StatsCard: React.FC<StatsCardProps> = ({ title, value, icon }) => {
  const getIcon = (iconName: string) => {
    switch (iconName) {
      case 'ShoppingBag': return <ShoppingBag className="text-blue-600" size={20} />;
      case 'Package': return <Package className="text-yellow-600" size={20} />;
      case 'AlertCircle': return <AlertCircle className="text-red-600" size={20} />;
      case 'Users': return <Users className="text-green-600" size={20} />;
      case 'DollarSign': return <DollarSign className="text-purple-600" size={20} />;
      default: return null;
    }
  };

  return (
    <div className="bg-white rounded-lg shadow p-4">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm text-gray-500">{title}</p>
          <p className="text-2xl font-bold">{typeof value === 'number' ? value : value}</p>
        </div>
        <div className={`p-3 rounded-full ${icon === 'ShoppingBag' ? 'bg-blue-100' : icon === 'Package' ? 'bg-yellow-100' : icon === 'AlertCircle' ? 'bg-red-100' : icon === 'Users' ? 'bg-green-100' : 'bg-purple-100'}`}>
          {getIcon(icon)}
        </div>
      </div>
    </div>
  );
};

export default StatsCard;