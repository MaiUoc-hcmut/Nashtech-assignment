import React from 'react';
import { Link } from 'react-router-dom';
import { ChevronLeft, ChevronRight, Home, ShoppingBag, Package, Grid, Layers, Tag, Star, Users, Settings, BarChart } from 'lucide-react';
import { MenuItem } from '../types/dashboardTypes';

interface SidebarProps {
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
  currentPage: string;
}

const Sidebar: React.FC<SidebarProps> = ({ 
  collapsed, 
  setCollapsed, 
  currentPage 
}) => {
  const menuItems: MenuItem[] = [
    { id: 'dashboard', label: 'Dashboard', icon: Home },
    { id: 'classifications', label: 'Classifications', icon: Grid },
    { id: 'parentCategories', label: 'ParentCategories', icon: Layers },
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
    <div className={`bg-black text-white transition-all duration-300 ${collapsed ? 'w-16' : 'w-64'} flex flex-col h-full`}>
      <div className="flex items-center justify-between h-16 px-4 border-b border-gray-800">
        {!collapsed && (
          <h1 className="text-xl font-bold">Cloths Admin</h1>
        )}
        <button 
          onClick={() => setCollapsed(!collapsed)}
          className="text-gray-400 hover:text-white focus:outline-none"
        >
          {collapsed ? <ChevronRight size={20} /> : <ChevronLeft size={20} />}
        </button>
      </div>
      
      <div className="flex-1 overflow-y-auto py-4">
        {menuItems.map((item) => (
          <Link 
            key={item.id}
            to={`/admin/${item.id}`}
            className={`flex items-center w-full px-4 py-3 transition-colors duration-200 
              ${currentPage === item.id ? 'bg-blue-600 text-white' : 'text-gray-300 hover:bg-gray-700'}
              ${collapsed ? 'justify-center' : 'justify-start'}`}
            title={collapsed ? item.label : ""}
          >
            <item.icon size={20} className="flex-shrink-0" />
            {!collapsed && <span className="ml-3">{item.label}</span>}
          </Link>
        ))}
      </div>
      
      {/* Settings at the bottom */}
      <div className="border-t border-gray-800 py-2">
        <Link 
          to={`/admin/${settingsItem.id}`}
          className={`flex items-center w-full px-4 py-3 transition-colors duration-200 
            ${currentPage === settingsItem.id ? 'bg-blue-600 text-white' : 'text-gray-300 hover:bg-gray-700'}
            ${collapsed ? 'justify-center' : 'justify-start'}`}
        >
          <settingsItem.icon size={20} className="flex-shrink-0" />
          {!collapsed && <span className="ml-3">{settingsItem.label}</span>}
        </Link>
      </div>
    </div>
  );
};

export default Sidebar;