import { createContext, useContext, useState, ReactNode } from 'react';

interface NavContextType {
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
}

const NavContext = createContext<NavContextType | undefined>(undefined);

export const NavProvider = ({ children }: { children: ReactNode }) => {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <NavContext.Provider value={{ collapsed, setCollapsed }}>
      {children}
    </NavContext.Provider>
  );
};

export const useNav = () => {
  const context = useContext(NavContext);
  if (context === undefined) {
    throw new Error('useNav must be used within a NavProvider');
  }
  return context;
};