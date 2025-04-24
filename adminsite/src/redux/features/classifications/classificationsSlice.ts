import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';

// Define types for your state
interface Classification {
  id: number;
  name: string;
  description: string;
}

interface ClassificationsState {
  classifications: Classification[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

// Initial state
const initialState: ClassificationsState = {
  classifications: [],
  status: 'idle',
  error: null,
};

// Create async thunks for API calls
export const fetchClassifications = createAsyncThunk(
  'classifications/fetchClassifications',
  async () => {
    const response = await fetch('http://localhost:5113/api/Classification');
    if (!response.ok) throw new Error('Failed to fetch classifications');
    return response.json();
  }
);

// Create your slice
const classificationsSlice = createSlice({
  name: 'classifications',
  initialState,
  reducers: {
    // Regular reducers here
    resetClassification: (state) => {
      state.classifications = [];
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchClassifications.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchClassifications.fulfilled, (state, action: PayloadAction<Classification[]>) => {
        state.status = 'succeeded';
        state.classifications = action.payload;
        state.error = null;
        console.log(action.payload);
      })
      .addCase(fetchClassifications.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      });
  },
});

// Export actions and reducer
export const { resetClassification } = classificationsSlice.actions;
export default classificationsSlice.reducer;