import React, { useEffect, useState } from "react";
import { groupBy } from "./utils/utils";
import "./custom.css";
import AddBooking from "./components/AddBooking";
import RemoveBooking from "./components/RemoveBooking";
import UpdateBooking from "./components/UpdateBooking";

const weekDays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

export default function App() {
   const [bookings, setBookings] = useState({});
    const [timeSlots, setTimeSlots] = useState([]);

  useEffect(() => {
        console.log("useEffect is running");
    (async () => {
        try {
        let [bookings, timeSlots] = await Promise.all([fetch("/booking").then((response) => response.json()), fetch("/booking/timeslots").then((response) => response.json())]);
            setBookings(groupBy(bookings, "day")); //Gruppera bokningarna efter dag och uppdatera tillståndet. Resultatet är ett objekt där varje nyckel är en dag (1-7) och värdet är en array av bokningar för den dagen.
        setTimeSlots(timeSlots);
      } catch (error) {
        console.error(error);
      }
    })();
  }, []);

    return (
        <div className="booking-table">
            <h1 className="page-title">Booking Schedule</h1>
            <div className="grid-container">
                {weekDays.map((dayName, i) => {
                    const day = i + 1;
                    const booking = bookings[day] || [];
                    return (
                        <div key={dayName} className="booking-row">
                            <div className="booking-title">{dayName}</div>
                            <div className="timeslot-list">
                                {timeSlots?.map((timeSlot) => {
                                    const { startTime, endTime } = timeSlot;
                                    const booker = Array.isArray(booking) ? booking.find(({ timeSlot }) => timeSlot.startTime === startTime && timeSlot.endTime === endTime) : null; // Vi använder Array.isArray() för att kontrollera om booking är en array. Om den är det används Array.prototype.find() för att söka igenom bokningarna. find() används för att hitta den första bokningen där timeSlot.startTime och timeSlot.endTime matchar de angivna värdena. Annars retunerar vi null eller ett enskildt objekt.
                                    return (
                                        <div key={`${dayName}_${startTime}_${endTime}`} className="booking-item">
                                            <span className="time-slot">
                                                {startTime} - {endTime}
                                            </span>
                                            {booker ? (
                                                <div className="addedbooking">
                                                    <span className="username">{booker?.user?.name}</span>
                                                    <RemoveBooking booking={booker} setBookings={setBookings} />
                                                    <UpdateBooking booking={booker} setBookings={setBookings} timeSlots={timeSlots} />
                                                </div>
                                            ) : (
                                                <AddBooking day={day} timeSlot={timeSlot} setBookings={setBookings} />
                                            )}
                                        </div>
                                    );
                                })}
                            </div>
                        </div>
                    );
                })}
            </div>
        </div>
    );
}
