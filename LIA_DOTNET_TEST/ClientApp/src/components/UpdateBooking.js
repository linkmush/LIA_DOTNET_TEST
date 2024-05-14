import React, { useState } from 'react';

const weekDays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

function UpdateBooking({ booking, setBookings, timeSlots }) {
    const [selectedTime, setSelectedTime] = useState('');
    const [selectedDay, setSelectedDay] = useState('');

    const handleUpdate = async (id) => {
        console.log("Updating booking...");
        if (!selectedDay || !selectedTime) {
            console.error("Both day and time need to be selected.");
            return;
        }

        try {
            const selectedSlot = timeSlots.find(slot => `${slot.startTime}-${slot.endTime}` === selectedTime);
            if (!selectedSlot) {
                console.error("Selected time slot not found");
                return;
            }

            console.log("Selected time slot:", selectedSlot);

            const updatedBooking = {
                ...booking,
                day: weekDays.indexOf(selectedDay) + 1, // Konvertera den valda dagen till motsvarande siffervärde (1-7).
                timeSlot: {
                    id: selectedSlot.id,
                    startTime: selectedSlot.startTime,
                    endTime: selectedSlot.endTime,
                }
            };

            console.log("Updated Booking:", updatedBooking);

            const response = await fetch(`/booking/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(updatedBooking),
            });

            if (response.ok) {
                // Uppdatera bokningarna efter att en bokning har uppdaterats
                const updatedBookingsResponse = await fetch("/booking");
                const updatedBookings = await updatedBookingsResponse.json();
                setBookings(updatedBookings);
                console.log("Updated bookings:", updatedBookings);

            } else {
                console.error("Failed to update booking");
            }
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div className="update-booking">
            <select className="select-field" value={selectedDay} onChange={(e) => setSelectedDay(e.target.value)}>
                <option value="">Day</option>
                {weekDays.map((day, index) => (
                    <option key={index} value={day}>
                        {day}
                    </option>
                ))}
            </select>
            <select className="select-field" value={selectedTime} onChange={(e) => setSelectedTime(e.target.value)}>
                <option value="">Time</option>
                {timeSlots.map((slot, index) => (
                    <option key={index} value={`${slot.startTime}-${slot.endTime}`}>
                        {slot.startTime} - {slot.endTime}
                    </option>
                ))}
            </select>
            <button className="update-button" onClick={() => handleUpdate(booking.id)}>Update Booking</button>
        </div>
    );
}

export default UpdateBooking;