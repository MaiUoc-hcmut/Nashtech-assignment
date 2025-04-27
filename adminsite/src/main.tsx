// index.tsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { Provider } from 'react-redux';
import { NavProvider } from './contexts/NavContext';
import { store } from './redux/store';
import App from './App';

const root = ReactDOM.createRoot(document.getElementById('root')!);
root.render(
  <React.StrictMode>
    <Provider store={store}>
      <NavProvider>
        <App />
      </NavProvider>
    </Provider>
  </React.StrictMode>
);