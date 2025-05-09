import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { Review } from '../../../types/globalTypes';
import axiosConfig from '../../config/axios.config';

interface ReviewsState {
  totalReviews: number;
  reviews: Review[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: ReviewsState = {
  totalReviews: 0,
  reviews: [],
  status: 'idle',
  error: null
};

// Async thunks
export const fetchReviews = createAsyncThunk(
  'reviews/fetchReviews',
  async (currentPage: number) => {
    const response = await axiosConfig.get(`http://localhost:5113/api/Review?pageNumber=${currentPage}&isAsc=false`);
    if (response.status !== 200) {
      throw new Error('Failed to fetch reviews');
    }
    return response.data;
  }
);

export const fetchReviewsByProducts = createAsyncThunk(
  'reviews/fetchReviewsByProducts',
  async (productIds: number[]) => {
    const queryParams = productIds.map(id => `productIds=${id}`).join('&');
    const response = await axiosConfig.get(`http://localhost:5113/api/Review?${queryParams}`);
    if (response.status !== 200) {
      throw new Error('Failed to fetch reviews by products');
    }
    return response.data;
  }
);

const reviewsSlice = createSlice({
  name: 'reviews',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch reviews cases
      .addCase(fetchReviews.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchReviews.fulfilled, (state, action: PayloadAction<{ totalReviews: number; reviews: Review[] }>) => {
        state.status = 'succeeded';
        state.totalReviews = action.payload.totalReviews;
        state.reviews = action.payload.reviews;
        state.error = null;
      })
      .addCase(fetchReviews.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Failed to fetch reviews';
      })
      
      // Fetch reviews by products cases
      .addCase(fetchReviewsByProducts.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchReviewsByProducts.fulfilled, (state, action: PayloadAction<Review[]>) => {
        state.status = 'succeeded';
        state.reviews = action.payload;
        state.error = null;
      })
      .addCase(fetchReviewsByProducts.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Failed to fetch reviews by products';
      });
  },
});

export default reviewsSlice.reducer;