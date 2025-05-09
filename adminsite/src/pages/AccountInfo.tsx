import React, { useState, useEffect } from "react";
import axiosConfig from '../redux/config/axios.config';
import { Admin } from "../types/globalTypes";
import LoadingSpinner from "../components/LoadingSpiner";

type InfoItem = {
  label: string;
  value: string;
};

const ProfilePage: React.FC = () => {
  const [admin, setAdmin] = useState<Admin | null>(null);
  const [info, setInfo] = useState<InfoItem[]>([]);
  const [originalInfo, setOriginalInfo] = useState<InfoItem[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [isPending, setIsPending] = useState(false);

  const [editingIndex, setEditingIndex] = useState<number | null>(null);
  const [editedValue, setEditedValue] = useState<string>("");

  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    const fetchAdminData = async () => {
      try {
        setLoading(true);
        const response = await axiosConfig.get("http://localhost:5113/api/Admin");
        console.log("Admin data:", response.data);
        setAdmin(response.data);
        
        // Create info items from admin data
        const adminInfo: InfoItem[] = [
          { label: "Name", value: response.data.name || "N/A" },
          { label: "Email", value: response.data.email || "N/A" },
          { label: "Phone number", value: response.data.phoneNumber || "N/A" },
          { label: "Address", value: response.data.address || "N/A" },
          { label: "Role", value: "Administrator" }, // Fixed role
          // Add other fields as needed based on your Admin type
        ];
        
        setInfo(adminInfo);
        setOriginalInfo(adminInfo);
      } catch (error) {
        console.error("Failed to fetch admin data:", error);
        // Set fallback data in case of error
        const fallbackInfo: InfoItem[] = [
          { label: "Name", value: "Error loading" },
          { label: "Email", value: "Error loading" },
          { label: "Phone number", value: "Error loading" },
          { label: "Address", value: "Error loading" },
          { label: "Role", value: "Administrator" },
        ];
        setInfo(fallbackInfo);
        setOriginalInfo(fallbackInfo);
      } finally {
        setLoading(false);
      }
    };
  
    fetchAdminData();
  }, []);

  // Explicitly compute the difference by comparing each item
  const isModified = info.some((item, index) => {
    return originalInfo[index]?.value !== item.value;
  });

  const handleEdit = (index: number) => {
    setEditingIndex(index);
    setEditedValue(info[index].value);
  };

  const handleCancel = () => {
    if (editingIndex !== null) {
      const updatedInfo = [...info];
      updatedInfo[editingIndex].value = originalInfo[editingIndex].value;
      setInfo(updatedInfo);
    }
    setEditingIndex(null);
    setEditedValue("");
  };

  const handleSave = (index: number) => {
    const updatedInfo = [...info];
    updatedInfo[index].value = editedValue;
    setInfo(updatedInfo);
    setEditingIndex(null);
    setEditedValue("");
    // Note: We don't update originalInfo here, so isModified will be true
    // and the Update button will appear
  };

  
  useEffect(() => {

  }, [isPending]);

  const handleUpdate = async () => {
    try {
      // Create an updated admin object based on the current info
      const updatedAdmin = { ...admin };
      
      // Update the admin object with the values from info
      info.forEach(item => {
        if (item.label === "Name") updatedAdmin.name = item.value;
        if (item.label === "Email") updatedAdmin.email = item.value;
        if (item.label === "Phone number") updatedAdmin.phoneNumber = item.value;
        if (item.label === "Address") updatedAdmin.address = item.value;
        // Note: Role is fixed as Administrator and not updated
      });
      
      // Send the updated admin data to the backend
      await axiosConfig.put(`http://localhost:5113/api/Admin/${admin?.id}`, updatedAdmin);
      
      setOriginalInfo(info);
      alert("Changes saved successfully!");
    } catch (error) {
      console.error("Failed to update admin data:", error);
      alert("Failed to save changes. Please try again.");
    }
  };

  const handlePasswordChange = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsPending(true);
    
    const form = e.currentTarget;
    const oldPassword = form.elements.namedItem('oldPassword') as HTMLInputElement;
    const newPassword = form.elements.namedItem('newPassword') as HTMLInputElement;
    const confirmPassword = form.elements.namedItem('confirmPassword') as HTMLInputElement;
    
    if (newPassword.value !== confirmPassword.value) {
      alert("New password and confirm password do not match!");
      return;
    }

    try {
      // Send password change request to the backend
      await axiosConfig.post(`http://localhost:5113/api/Admin/changePassword`, {
        OldPassword: oldPassword.value,
        NewPassword: newPassword.value,
        ConfirmPassword: confirmPassword.value
      });
      
      setIsModalOpen(false);
    } catch (error) {
      console.error("Failed to change password:", error);
      alert("Failed to change password. Please check your old password and try again.");
    } finally {
      setIsPending(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-transparent flex items-center justify-center">
        <div className="text-xl">Loading admin information...</div>
      </div>
    );
  }

  if (isPending) {
    return <LoadingSpinner />;
  }

  return (
    <div className="min-h-screen bg-transparent flex items-center justify-center p-8">
      <div className="w-full max-w-2xl space-y-6">
        {/* Info Rows */}
        {info.map((item, index) => (
          <div
            key={item.label}
            className="flex justify-between items-center px-4 h-20"
          >
            <div>
              <div className="text-base text-gray-500">{item.label}</div>
              {editingIndex === index && item.label !== "Role" ? (
                <input
                  value={editedValue}
                  onChange={(e) => setEditedValue(e.target.value)}
                  className="text-xl font-semibold text-black border border-gray-300 px-2 py-1 rounded w-64"
                />
              ) : (
                <div className="text-xl font-semibold text-black">{item.value}</div>
              )}
            </div>
            <div className="space-x-2">
              {editingIndex === index ? (
                <>
                  <button
                    onClick={handleCancel}
                    className="border border-gray-400 text-gray-700 px-4 py-2 rounded hover:bg-gray-100"
                  >
                    Cancel
                  </button>
                  <button
                    onClick={() => handleSave(index)}
                    className="border border-blue-500 text-blue-500 px-4 py-2 rounded hover:bg-blue-50"
                  >
                    Save
                  </button>
                </>
              ) : (
                item.label !== "Role" && (
                  <button
                    onClick={() => handleEdit(index)}
                    className="border border-blue-500 text-blue-500 px-5 py-2 rounded hover:bg-blue-50"
                  >
                    Edit
                  </button>
                )
              )}
            </div>
          </div>
        ))}

        {/* Bottom Buttons */}
        <div className="flex justify-center gap-4 mt-8">
          <button
            onClick={() => setIsModalOpen(true)}
            className="bg-blue-800 text-white px-6 py-3 rounded text-lg hover:bg-blue-900 transition"
          >
            Change Password
          </button>
          {isModified && (
            <button
              onClick={handleUpdate}
              className="bg-gray-700 text-white px-6 py-3 rounded text-lg hover:bg-gray-800 transition"
            >
              Update
            </button>
          )}
        </div>
      </div>

      {/* Modal */}
      {isModalOpen && (
        <div
          className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm"
          onClick={() => setIsModalOpen(false)}
        >
          <div
            className="bg-white rounded-lg p-6 w-full max-w-md"
            onClick={(e) => e.stopPropagation()}
          >
            <h2 className="text-2xl font-semibold mb-4">Change Password</h2>
            <form className="space-y-4" onSubmit={handlePasswordChange}>
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Old Password
                </label>
                <input
                  name="oldPassword"
                  type="password"
                  required
                  className="w-full mt-1 px-3 py-2 border rounded focus:outline-none focus:ring focus:ring-blue-300"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  New Password
                </label>
                <input
                  name="newPassword"
                  type="password"
                  required
                  className="w-full mt-1 px-3 py-2 border rounded focus:outline-none focus:ring focus:ring-blue-300"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Confirm Password
                </label>
                <input
                  name="confirmPassword"
                  type="password"
                  required
                  className="w-full mt-1 px-3 py-2 border rounded focus:outline-none focus:ring focus:ring-blue-300"
                />
              </div>
              <div className="flex justify-end space-x-2 pt-4">
                <button
                  type="button"
                  onClick={() => setIsModalOpen(false)}
                  className="px-4 py-2 rounded border border-gray-300 hover:bg-gray-100"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700"
                >
                  Save
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfilePage;