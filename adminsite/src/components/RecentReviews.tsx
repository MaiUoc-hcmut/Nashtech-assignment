import { Star } from 'lucide-react';
import { RecentReview } from '../types/dashboardTypes';

interface RecentReviewsProps {
  reviews: RecentReview[];
}

const RecentReviews: React.FC<RecentReviewsProps> = ({ reviews }) => {
  return (
    <div className="bg-white rounded-lg shadow p-6">
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-xl font-semibold">Recent Reviews</h2>
        <button className="text-sm text-blue-600 hover:text-blue-800">View All</button>
      </div>
      <div className="space-y-4">
        {reviews.map(review => (
          <div key={review.id} className="border-b border-gray-100 pb-4">
            <div className="flex justify-between items-center">
              <p className="font-medium">{review.product}</p>
              <div className="flex items-center">
                {Array(5).fill(0).map((_, i) => (
                  <Star 
                    key={i} 
                    size={14} 
                    className={i < review.rating ? "text-yellow-400 fill-yellow-400" : "text-gray-300"} 
                  />
                ))}
              </div>
            </div>
            <p className="text-sm text-gray-500 mt-1">{review.comment}</p>
            <p className="text-xs text-gray-400 mt-2">By {review.customer}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default RecentReviews;