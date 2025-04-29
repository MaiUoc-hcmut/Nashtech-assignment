const StarRating: React.FC<{ rating: number }> = ({ rating }) => {
  const stars = Array.from({ length: 5 }, (_, i) => {
    const filled = i < Math.floor(rating);
    const halfFilled = i === Math.floor(rating) && rating % 1 >= 0.5;
    
    return (
      <span key={i} className="text-yellow-400">
        {filled ? (
          '★'
        ) : halfFilled ? (
          '★' // You could use a half-star icon here if available
        ) : (
          '☆'
        )}
      </span>
    );
  });

  return <div className="flex">{stars}</div>;
};

export default StarRating;