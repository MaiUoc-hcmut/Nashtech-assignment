import React, { useState, useRef, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ChevronLeft, ChevronRight, Home, ShoppingBag, Package, Grid, Layers, Tag, Star, Users, Settings, Info, LogOut, ChevronRight as ChevronRightSmall } from 'lucide-react';
import { MenuItem } from '../types/dashboardTypes';
import { useAppDispatch } from '../redux/hooks/redux';
import { logout } from '../redux/features/auths/authsSlice';
import { useNavigate } from 'react-router-dom';

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
  const [settingsOpen, setSettingsOpen] = useState(false);
  const [showAbove, setShowAbove] = useState(false);
  const settingsRef = useRef<HTMLDivElement>(null);
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [isLogout, setIsLogout] = useState(false);
  
  // Check if settings item is near the bottom of the viewport
  useEffect(() => {
    if (settingsOpen && settingsRef.current) {
      const rect = settingsRef.current.getBoundingClientRect();
      const windowHeight = window.innerHeight;
      const bottomSpace = windowHeight - rect.bottom;
      
      // If there's not enough space below (less than 150px), show menu above
      setShowAbove(bottomSpace < 150);
    }
  }, [settingsOpen]);
  
  const menuItems: MenuItem[] = [
    { id: 'dashboard', label: 'Dashboard', icon: Home },
    { id: 'classifications', label: 'Classifications', icon: Grid },
    { id: 'parentCategories', label: 'ParentCategories', icon: Layers },
    { id: 'categories', label: 'Categories', icon: Tag },
    { id: 'products', label: 'Products', icon: ShoppingBag },
    { id: 'orders', label: 'Orders', icon: Package },
    { id: 'reviews', label: 'Reviews', icon: Star },
    { id: 'customers', label: 'Customers', icon: Users }
  ];

  const toggleSettingsMenu = (e: React.MouseEvent) => {
    e.preventDefault();
    setSettingsOpen(!settingsOpen);
  };

  const handleLogout = async () => {
    try {
      await dispatch(logout()).unwrap();
      setSettingsOpen(false);
      setIsLogout(true);
      navigate('/login', { replace: true });
    } catch (error) {
      console.error('Failed to logout:', error);
    }
  };

  useEffect(() => {

  }, [isLogout]);

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
      
      {/* Settings with submenu */}
      <div className="border-t border-gray-800 py-2">
        <div className="relative" ref={settingsRef}>
          <a 
            href="#"
            onClick={toggleSettingsMenu}
            className={`flex items-center w-full px-4 py-3 transition-colors duration-200 
              ${['profile', 'information', 'logout'].includes(currentPage) ? 'bg-blue-600 text-white' : 'text-gray-300 hover:bg-gray-700'}
              ${collapsed ? 'justify-center' : 'justify-between'}`}
          >
            <div className="flex items-center">
              <Settings size={20} className="flex-shrink-0" />
              {!collapsed && <span className="ml-3">Settings</span>}
            </div>
            {!collapsed && (
              <div>
                <ChevronRightSmall size={16} />
              </div>
            )}
          </a>
          
          {/* Submenu - show on the right side as a flyout for both collapsed and expanded states */}
          {settingsOpen && (
            <div className={`absolute left-full ${showAbove ? 'bottom-full mb-2' : 'top-0 mt-0'} ml-2 bg-gray-900 rounded shadow-lg py-1 z-10 w-48`}>
              <Link
                to="/admin/information"
                className="flex items-center w-full px-4 py-2 text-gray-300 hover:bg-gray-700"
              >
                <Info size={18} className="flex-shrink-0" />
                <span className="ml-3">Information</span>
              </Link>
              <div
                style={{cursor: "pointer"}}
                className="flex items-center w-full px-4 py-2 text-gray-300 hover:bg-gray-700"
                onClick={handleLogout}
              >
                <LogOut size={18} className="flex-shrink-0" />
                <span className="ml-3">Logout</span>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Sidebar;