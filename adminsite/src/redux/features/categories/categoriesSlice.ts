import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import axiosConfig from '../../config/axios.config';
import { ParentCategory } from '../../../types/globalTypes';

// Define types for your state
interface Category {
  id: number;
  name: string;
  description: string;
  parentCategory?: ParentCategory
}

interface AddCategory {
  name: string;
  parent: number
  description: string;
}

interface CategoriesState {
  categories: Category[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

// Initial state
const initialState: CategoriesState = {
  categories: [],
  status: 'idle',
  error: null,
};

// Create async thunks for API calls
export const fetchCategories = createAsyncThunk(
  'categories/fetchCategories',
  async () => {
    const response = await axiosConfig.get('http://localhost:5113/api/Category');
    if (response.status !== 200) throw new Error('Failed to fetch categories');
    return response.data;
  }
);

export const addCategory = createAsyncThunk(
  'categories/addCategory',
  async(payload: AddCategory) => {
    const { parent, ...data } = payload;
    const response = await axiosConfig.post(`http://localhost:5113/api/Category?parentId=${parent}`, data);
    if (response.status !== 200) throw new Error('Failed to add categories');
    return response.data;
  }
);

export const updateCategory = createAsyncThunk(
  'categories/updateCategory',
  async(payload: Category) => {
    const { id, ...categoryData } = payload;
    const response = await axiosConfig.put(`http://localhost:5113/api/Category/${id}`, categoryData);
    if (response.status !== 200) throw new Error('Failed to edit categories');
    return response.data;
  }
);

export const deleteCategory = createAsyncThunk(
  'categories/deleteCategory',
  async(id: number) => {
    const response = await axiosConfig.delete(`http://localhost:5113/api/Category/${id}`);
    if (response.status !== 200) throw new Error('Failed to edit categories');
    return id;
  }
);

// Create your slice
const categoriesSlice = createSlice({
  name: 'categories',
  initialState,
  reducers: {
    // Regular reducers here
    resetCategories: (state) => {
      state.categories = [];
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchCategories.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchCategories.fulfilled, (state, action: PayloadAction<Category[]>) => {
        state.status = 'succeeded';
        state.categories = action.payload;
        state.error = null;
      })
      .addCase(fetchCategories.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(addCategory.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(addCategory.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.categories.push(action.payload);
        state.error = null;
        console.log(action.payload);
      })
      .addCase(addCategory.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(updateCategory.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(updateCategory.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.error = null;

        const index = state.categories.findIndex(
          (category) => category.id === action.payload.id
        );
        if (index !== -1) {
          state.categories[index] = action.payload;
        }
      })
      .addCase(updateCategory.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(deleteCategory.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(deleteCategory.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.error = null;

        const index = state.categories.findIndex(
          (category) => category.id === action.payload
        );
        if (index !== -1) {
          state.categories.splice(index, 1);
        }
      })
      .addCase(deleteCategory.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      });
  },
});

// Export actions and reducer
export const { resetCategories } = categoriesSlice.actions;
export default categoriesSlice.reducer;