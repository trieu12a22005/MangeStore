const mongoose = require("mongoose");

const ReservationSchema = new mongoose.Schema({
    customerName: {
        type: String,
        required: true,
        trim: true
    },
    phoneNumber: {
        type: String,
        required: true,
        trim: true
    },
    numberOfGuests: {
        type: Number,
        required: true,
        min: 1
    },
    reservationDate: {
        type: Date,
        required: true
    },
    reservationTime: {
        type: String,
        required: true
    },
    tableNumber: {
        type: String,
        required: false, // Có thể đặt trước hoặc để nhà hàng gán sau
        trim: true
    },
    status: {
        type: String,
        enum: ["Pending", "Confirmed", "Cancelled", "Completed"],
        default: "Pending"
    },
    createdAt: {
        type: Date,
        default: Date.now
    }
});
const Reservation = mongoose.model("Reservation", ReservationSchema,"Reservation");
module.exports = Reservation;
