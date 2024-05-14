import React from 'react';

const AddBooking = ({ day, timeSlot, setBookings }) => {
    const handleAddBooking = async () => {
        const userName = prompt("Enter your name:");
        if (userName) {
            try {
                const response = await fetch("/booking", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        day,
                        user: { name: userName },
                        timeSlot,
                    }),
                });
                if (response.ok) {
                    const newBooking = await response.json();
                    setBookings((prevBookings) => {
                        const updatedBookings = { ...prevBookings };
                        if (!updatedBookings[day]) {
                            updatedBookings[day] = [newBooking];
                        } else {
                            updatedBookings[day] = [...updatedBookings[day], newBooking];
                        }
                        return updatedBookings;
                    });
                } else {
                    console.error("Failed to add booking");
                }
            } catch (error) {
                console.error(error);
            }
        }
    };

    return (
        <button className="addbooking" onClick={handleAddBooking}>Add booking</button>
    );
};

export default AddBooking;