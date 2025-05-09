import React, { useState, useEffect, ChangeEvent, FocusEvent } from 'react';
import { Bold, Italic, Heading, List, Type, Palette } from 'lucide-react';

// Define the props interface for the component
interface RichTextEditorProps {
  value: string;
  onChange: (event: { target: { value: string } }) => void;
  onSubmit?: (htmlContent: string) => void; // New prop for submitting HTML data
  onBlur?: (event: FocusEvent<HTMLTextAreaElement>) => void;
  error?: boolean;
  errorMessage?: string;
  label?: string;
  id?: string;
  className?: string;
}

const RichTextEditor: React.FC<RichTextEditorProps> = ({ 
  value, 
  onChange, 
  // onSubmit,
  onBlur, 
  error = false,
  errorMessage, 
  label = 'Description',
  id = 'rich-editor',
  className = ''
}) => {
  const [showColorPicker, setShowColorPicker] = useState<boolean>(false);
  const [selectedColor, setSelectedColor] = useState<string>('#000000');
  
  useEffect(() => {
    // Close color picker when clicking outside
    const handleClickOutside = (e: MouseEvent) => {
      if (!(e.target as Element).closest('.color-picker-container')) {
        setShowColorPicker(false);
      }
    };
    
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  // Function to convert markdown-like syntax to HTML
  const convertToHTML = (text: string): string => {
    return text
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>')
      .replace(/^## (.*?)$/gm, '<h2>$1</h2>')
      .replace(/^- (.*?)$/gm, '<li>$1</li>');
  };

  const formatText = (tag: 'bold' | 'italic' | 'heading' | 'list' | 'color') => {
    const textarea = document.getElementById(id) as HTMLTextAreaElement;
    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;
    const selectedText = value.substring(start, end);
    
    let formattedText = '';
    let cursorPosition = 0;
    
    switch(tag) {
      case 'bold':
        formattedText = `**${selectedText}**`;
        cursorPosition = start + formattedText.length;
        break;
      case 'italic':
        formattedText = `*${selectedText}*`;
        cursorPosition = start + formattedText.length;
        break;
      case 'heading':
        formattedText = `## ${selectedText}`;
        cursorPosition = start + formattedText.length;
        break;
      case 'list':
        formattedText = `- ${selectedText}`;
        cursorPosition = start + formattedText.length;
        break;
      case 'color':
        formattedText = `<span style="color:${selectedColor}">${selectedText}</span>`;
        cursorPosition = start + formattedText.length;
        break;
      default:
        return;
    }
    
    const newContent = value.substring(0, start) + formattedText + value.substring(end);
    onChange({ target: { value: newContent } });
    
    setTimeout(() => {
      textarea.focus();
      textarea.selectionStart = cursorPosition;
      textarea.selectionEnd = cursorPosition;
    }, 0);
  };

  const changeTextSize = (increase: boolean) => {
    const textarea = document.getElementById(id) as HTMLTextAreaElement;
    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;
    const selectedText = value.substring(start, end);
    
    const size = increase ? 'lg' : 'sm';
    const formattedText = `<span class="text-${size}">${selectedText}</span>`;
    
    const newContent = value.substring(0, start) + formattedText + value.substring(end);
    onChange({ target: { value: newContent } });
    
    setTimeout(() => {
      textarea.focus();
      textarea.selectionStart = start + formattedText.length;
      textarea.selectionEnd = start + formattedText.length;
    }, 0);
  };

  // const handleSubmit = () => {
  //   if (onSubmit) {
  //     const htmlContent = convertToHTML(value);
  //     onSubmit(htmlContent);
  //   }
  // };

  const colors: string[] = [
    '#000000', '#ff0000', '#0000ff', '#008000', '#800080', '#ffa500', '#a52a2a', '#808080'
  ];

  return (
    <div className={`${className} space-y-2`}>
      <label className="block text-sm font-medium text-gray-700">{label}</label>
      
      <div className="flex flex-wrap gap-2 p-2 border border-gray-300 rounded-t-md bg-gray-50">
        <button 
          type="button"
          onClick={() => formatText('bold')}
          className="p-1 hover:bg-gray-200 rounded"
          title="Bold"
        >
          <Bold size={18} />
        </button>
        <button 
          type="button"
          onClick={() => formatText('italic')}
          className="p-1 hover:bg-gray-200 rounded"
          title="Italic"
        >
          <Italic size={18} />
        </button>
        <button 
          type="button"
          onClick={() => formatText('heading')}
          className="p-1 hover:bg-gray-200 rounded"
          title="Heading"
        >
          <Heading size={18} />
        </button>
        <button 
          type="button"
          onClick={() => formatText('list')}
          className="p-1 hover:bg-gray-200 rounded"
          title="Bullet List"
        >
          <List size={18} />
        </button>
        <div className="h-6 w-px bg-gray-300 mx-1"></div>
        <button 
          type="button"
          onClick={() => changeTextSize(false)}
          className="p-1 hover:bg-gray-200 rounded"
          title="Decrease Text Size"
        >
          <Type size={14} />
        </button>
        <button 
          type="button"
          onClick={() => changeTextSize(true)}
          className="p-1 hover:bg-gray-200 rounded"
          title="Increase Text Size"
        >
          <Type size={20} />
        </button>
        <div className="h-6 w-px bg-gray-300 mx-1"></div>
        <div className="relative color-picker-container">
          <button 
            type="button"
            onClick={() => setShowColorPicker(!showColorPicker)}
            className="p-1 hover:bg-gray-200 rounded flex items-center"
            title="Text Color"
          >
            <Palette size={18} />
            <div 
              className="w-3 h-3 ml-1 rounded-full" 
              style={{ backgroundColor: selectedColor }}
            ></div>
          </button>
          
          {showColorPicker && (
            <div className="absolute top-8 left-0 z-10 p-2 bg-white shadow-lg rounded border border-gray-200 flex flex-wrap gap-1 w-32">
              {colors.map((color) => (
                <div
                  key={color}
                  className="w-6 h-6 rounded-full cursor-pointer hover:scale-110 transition-transform border border-gray-300"
                  style={{ backgroundColor: color }}
                  onClick={() => {
                    setSelectedColor(color);
                    setShowColorPicker(false);
                    formatText('color');
                  }}
                ></div>
              ))}
            </div>
          )}
        </div>
      </div>
      
      <textarea
        id={id}
        className={`block w-full border ${error ? 'border-red-500' : 'border-gray-300'} rounded-b-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500`}
        value={value}
        onChange={onChange as (event: ChangeEvent<HTMLTextAreaElement>) => void}
        onBlur={onBlur}
      />
      
      {error && errorMessage && (
        <p className="text-red-500 text-sm mt-1">{errorMessage}</p>
      )}
      
      <div className="mt-2">
        <h3 className="text-sm font-medium text-gray-700 mb-1">Preview:</h3>
        <div 
          className="p-3 border border-gray-300 rounded-md min-h-16 text-sm prose"
          dangerouslySetInnerHTML={{ 
            __html: convertToHTML(value)
          }}
        ></div>
      </div>

      {/* <button
        type="button"
        onClick={handleSubmit}
        className="mt-2 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
      >
        Submit
      </button> */}
    </div>
  );
};

export default RichTextEditor;