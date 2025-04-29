import { configureStore } from '@reduxjs/toolkit';
import parentCategoriesReducer from '../features/parentCategories/parentCategoriesSlice';
import categoriesReducer from '../features/categories/categoriesSlice';
import classificationReducer from '../features/classifications/classificationsSlice';
import authReducer from '../features/auths/authsSlice';
import productReducer from '../features/products/productsSlice';

export const store = configureStore({
  reducer: {
    categories: categoriesReducer,
    parentCategories: parentCategoriesReducer,
    classifications: classificationReducer,
    auth: authReducer,
    products: productReducer
  },
});

// Export types for TypeScript
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;