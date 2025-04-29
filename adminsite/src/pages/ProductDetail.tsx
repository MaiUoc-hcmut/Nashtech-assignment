import React from 'react';
import { useParams } from 'react-router-dom';

const ProductDetail: React.FC = () => {
  const { id } = useParams();

  return (
    <div className="p-6 bg-gray-100 border-t">
      <h1 className="text-2xl font-bold">Product Detail</h1>
      <p>Product ID: {id}</p>
      {/* Add your product detail content here */}
    </div>
  );
};

export default ProductDetail;