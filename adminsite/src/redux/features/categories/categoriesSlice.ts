import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';

// Define types for your state
interface Category {
  id: number;
  name: string;
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
    const response = await fetch('http://localhost:5113/api/Category');
    if (!response.ok) throw new Error('Failed to fetch categories');
    return response.json();
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
        console.log(action.payload);
      })
      .addCase(fetchCategories.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      });
  },
});

// Export actions and reducer
export const { resetCategories } = categoriesSlice.actions;
export default categoriesSlice.reducer;