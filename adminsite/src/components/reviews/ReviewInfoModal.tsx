
interface ReviewInfoModalProps {
    isOpen: boolean;
    onClose: () => void;
}

const ReviewInfoModal: React.FC<ReviewInfoModalProps> = ({isOpen, onClose}) => {
    if (!isOpen) return null;
    return (
        <div>
            <button onClick={onClose}>X</button>
        </div>
    )
}

export default ReviewInfoModal;