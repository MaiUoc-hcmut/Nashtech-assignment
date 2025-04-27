import { configureStore } from '@reduxjs/toolkit';
import categoriesReducer from '../features/categories/categoriesSlice';
import classificationReducer from '../features/classifications/classificationsSlice';
import authReducer from '../features/auths/authsSlice';

export const store = configureStore({
  reducer: {
    categories: categoriesReducer,
    classifications: classificationReducer,
    auth: authReducer,
  },
});

// Export types for TypeScript
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;