// import { useEffect } from 'react';
import { ChevronLeft, ChevronRight, Home, ShoppingBag, Package, Grid, Tag, Star, Users, Settings, BarChart } from 'lucide-react';
import { MenuItem } from '../types/dashboardTypes';

interface SidebarProps {
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
  currentPage: string;
  setCurrentPage: (page: string) => void;
}

const Sidebar: React.FC<SidebarProps> = ({ 
  collapsed, 
  setCollapsed, 
  currentPage, 
  setCurrentPage 
}) => {
  const menuItems: MenuItem[] = [
    { id: 'dashboard', label: 'Dashboard', icon: Home },
    { id: 'classifications', label: 'Classifications', icon: Grid },
    { id: 'categories', label: 'Categories', icon: Tag },
    { id: 'products', label: 'Products', icon: ShoppingBag },
    { id: 'orders', label: 'Orders', icon: Package },
    { id: 'reviews', label: 'Reviews', icon: Star },
    { id: 'customers', label: 'Customers', icon: Users },
    { id: 'analytics', label: 'Analytics', icon: BarChart }
  ];

  // Settings menu item separate from the main navigation
  const settingsItem: MenuItem = { id: 'settings', label: 'Settings', icon: Settings };

  return (
    <div className={`h-full flex flex-col bg-gray-800 text-white transition-all duration-300 ${collapsed ? 'w-16' : 'w-64'} z-10`}>
      <div className="flex items-center justify-between h-16 px-4 bg-gray-900">
        {!collapsed && <h1 className="text-xl font-bold">Cloths Admin</h1>}
        <button 
          onClick={() => setCollapsed(!collapsed)}
          className="text-gray-400 hover:text-white focus:outline-none"
        >
          {collapsed ? <ChevronRight size={20} /> : <ChevronLeft size={20} />}
        </button>
      </div>
      
      <div className="flex-1 mt-6">
        {menuItems.map((item) => (
          <button
            key={item.id}
            onClick={() => setCurrentPage(item.id)}
            className={`flex items-center w-full px-4 py-3 transition-colors duration-200 
              ${currentPage === item.id ? 'bg-blue-600 text-white' : 'text-gray-300 hover:bg-gray-700'}
              ${collapsed ? 'justify-center' : 'justify-start'}`}
          >
            <item.icon size={20} className={collapsed ? '' : 'mr-3'} />
            {!collapsed && <span>{item.label}</span>}
          </button>
        ))}
      </div>
      
      {/* Settings at the bottom */}
      <div className="mt-auto mb-6 border-t border-gray-700 pt-4">
        <button
          onClick={() => setCurrentPage(settingsItem.id)}
          className={`flex items-center w-full px-4 py-3 transition-colors duration-200 
            ${currentPage === settingsItem.id ? 'bg-blue-600 text-white' : 'text-gray-300 hover:bg-gray-700'}
            ${collapsed ? 'justify-center' : 'justify-start'}`}
        >
          <settingsItem.icon size={20} className={collapsed ? '' : 'mr-3'} />
          {!collapsed && <span>{settingsItem.label}</span>}
        </button>
      </div>
    </div>
  );
};

export default Sidebar;