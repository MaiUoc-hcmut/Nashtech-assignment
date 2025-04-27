// src/contexts/NavContext.tsx
import { useState, createContext, useContext, ReactNode } from 'react';

interface NavContextType {
  currentPage: string;
  setCurrentPage: (page: string) => void;
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
}

// Create context with default values
const NavContext = createContext<NavContextType | null>(null);

// Provider component
export const NavProvider = ({ children }: { children: ReactNode }) => {
  const [currentPage, setCurrentPage] = useState('dashboard');
  const [collapsed, setCollapsed] = useState(false);
  
  return (
    <NavContext.Provider value={{ currentPage, setCurrentPage, collapsed, setCollapsed }}>
      {children}
    </NavContext.Provider>
  );
};

// Custom hook to use the context
export const useNav = (): NavContextType => {
  const context = useContext(NavContext);
  if (!context) {
    throw new Error('useNav must be used within a NavProvider');
  }
  return context;
};