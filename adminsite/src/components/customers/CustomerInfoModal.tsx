import { Customer } from "../../types/globalTypes";

interface CustomerInfoModalProps {
    isOpen: boolean;
    onClose: () => void;
    customer: Customer | null;
}

const CustomerInfoModal: React.FC<CustomerInfoModalProps> = ({isOpen, onClose, customer}) => {
    if (!isOpen) return null;
    return (
        <div>
            <button onClick={onClose}>X</button>
        </div>
    )
}

export default CustomerInfoModal;