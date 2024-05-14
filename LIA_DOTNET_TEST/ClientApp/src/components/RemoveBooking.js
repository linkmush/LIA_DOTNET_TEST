import React from 'react';

const RemoveBooking = ({ booking, setBookings }) => {
    const handleRemoveBooking = async () => {
        if (window.confirm("Are you sure you want to remove this booking?")) {
            try {
                const response = await fetch(`/booking`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(booking),
                });

                if (response.ok) {
                    // Uppdatera UI för att reflektera booking removal
                    setBookings((prevBookings) => {
                        const updatedBookings = { ...prevBookings };  // här använder jag mig av spridningssyntaxen "...", för att säkerställa smidighet på sidan lokalt medans den faktiskt databas ändringen hanteras i bakrunden tills den begäran är "klar"
                        const dayBookings = updatedBookings[booking.day];
                        const filteredBookings = dayBookings.filter(
                            (b) =>
                                !(b.timeSlot.startTime === booking.timeSlot.startTime && b.timeSlot.endTime === booking.timeSlot.endTime)
                        );
                        updatedBookings[booking.day] = filteredBookings;
                        return updatedBookings;
                    });
                } else {
                    console.error("Failed to remove booking");
                }
            } catch (error) {
                console.error(error);
            }
        }
    };

    return (
        <button className="button-red" onClick={handleRemoveBooking}>Remove booking</button>
    );
}

export default RemoveBooking;