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
                        const updatedBookings = { ...prevBookings };  // här använder jag mig av spridningssyntaxen "...", för att säkerställa smidighet på sidan lokalt medans den faktiskt databas ändringen hanteras i bakrunden tills den begäran är "klar"
                        if (!updatedBookings[day]) { // Om det inte finns några bokningar för den angivna dagen...
                            updatedBookings[day] = [newBooking]; // Skapa en ny array med den nya bokningen för den angivna dagen(1-7)
                        } else {
                            updatedBookings[day] = [...updatedBookings[day], newBooking];     // Om det redan finns bokningar för den angivna dagen.. Skapa en kopia av den befintliga arrayen och lägg till den nya bokningen i den kopian sedan skickas till databasen.
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