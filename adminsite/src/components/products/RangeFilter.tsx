import { useState, useEffect } from 'react';
import './styles/rangeSlider.css';

export interface RangeFilterProps {
  icon: React.ReactNode;
  title: string;
  minValue: number;
  maxValue: number;
  step: number;
  currentRange: { min: number; max: number };
  setRange: (range: { min: number; max: number }) => void;
  formatValue?: (value: number) => string;
}

const RangeFilter: React.FC<RangeFilterProps> = ({
  icon,
  title,
  minValue,
  maxValue,
  step,
  currentRange,
  setRange,
}) => {
  // Local state for input values before applying filters
  const [rangeInput, setRangeInput] = useState(currentRange);

  // Update local input state when props change
  useEffect(() => {
    setRangeInput(currentRange);
  }, [currentRange]);

  // Handle range input changes
  const handleRangeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const parsedValue = parseFloat(value);
    
    // Update both the actual range and the input range
    if (name === 'min') {
      const newMin = Math.min(parsedValue, currentRange.max - step);
      setRange({ ...currentRange, min: newMin });
      setRangeInput({ ...rangeInput, min: newMin });
    } else {
      const newMax = Math.max(parsedValue, currentRange.min + step);
      setRange({ ...currentRange, max: newMax });
      setRangeInput({ ...rangeInput, max: newMax });
    }
  };

  // Handle number input changes (without immediate apply)
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const parsedValue = parseFloat(value);
    
    setRangeInput({
      ...rangeInput,
      [name]: parsedValue
    });
  };

  // Apply the input values to the actual range
  const applyFilter = () => {
    // Ensure min is not greater than max
    const validRange = {
      min: Math.min(rangeInput.min, rangeInput.max - step),
      max: Math.max(rangeInput.min + step, rangeInput.max)
    };
    setRange(validRange);
    setRangeInput(validRange);
  };

  // Calculate the percentage for slider thumbs positioning
  const minThumbPosition = ((currentRange.min - minValue) / (maxValue - minValue)) * 100;
  const maxThumbPosition = ((currentRange.max - minValue) / (maxValue - minValue)) * 100;

  return (
    <div>
      <h4 className="text-sm font-medium mb-2 flex items-center">
        {icon}
        {title}
      </h4>
      
      {/* Range slider with custom CSS classes */}
      <div className="range-slider mb-4">
        {/* Track background */}
        <div className="track-bg"></div>
        
        {/* Active track section */}
        <div 
          className="track-active"
          style={{
            left: `${minThumbPosition}%`,
            width: `${maxThumbPosition - minThumbPosition}%`
          }}
        ></div>
        
        {/* Min thumb */}
        <input
          type="range"
          name="min"
          min={minValue}
          max={maxValue}
          step={step}
          value={currentRange.min}
          onChange={handleRangeChange}
          className="min-range"
        />
        
        {/* Max thumb */}
        <input
          type="range"
          name="max"
          min={minValue}
          max={maxValue}
          step={step}
          value={currentRange.max}
          onChange={handleRangeChange}
          className="max-range"
        />
      </div>
      
      {/* Input fields */}
      <div className="flex space-x-2">
        <div>
          <label className="block text-xs text-gray-500 mb-1">Min</label>
          <input
            type="number"
            name="min"
            min={minValue}
            max={maxValue}
            step={step}
            value={rangeInput.min}
            onChange={handleInputChange}
            className="w-full p-2 border border-gray-300 rounded-md text-sm"
          />
        </div>
        <div>
          <label className="block text-xs text-gray-500 mb-1">Max</label>
          <input
            type="number"
            name="max"
            min={minValue}
            max={maxValue}
            step={step}
            value={rangeInput.max}
            onChange={handleInputChange}
            className="w-full p-2 border border-gray-300 rounded-md text-sm"
          />
        </div>
      </div>
      
      {/* Apply button */}
      <button
        onClick={applyFilter}
        className="mt-2 text-xs bg-blue-100 hover:bg-blue-200 text-blue-800 px-2 py-1 rounded"
      >
        Apply
      </button>
    </div>
  );
};

export default RangeFilter;