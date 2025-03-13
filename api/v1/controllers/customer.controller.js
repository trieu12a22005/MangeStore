const customer = require("../models/customer.model");
module.exports.postReversation = async(req,res) =>{
    try {
        const { customerName, phoneNumber, numberOfGuests, reservationDate, reservationTime, tableNumber } = req.body;

        // Kiểm tra dữ liệu đầu vào
        if (!customerName || !phoneNumber || !numberOfGuests || !reservationDate || !reservationTime) {
            return res.status(400).json({ message: "Missing required fields" });
        }

        // Kiểm tra số khách hợp lệ
        if (numberOfGuests < 1) {
            return res.status(400).json({ message: "Number of guests must be at least 1" });
        }

        // Kiểm tra định dạng ngày
        const date = new Date(reservationDate);
        if (isNaN(date.getTime())) {
            return res.status(400).json({ message: "Invalid reservation date" });
        }
        const table = await customer.find(
            {
                tableNumber: tableNumber
            }
        )
        if (tableNumber)
        {
            return res.json({
                code: 400,
                message: "Bàn đã được đặt"
            })
        }
        // Tạo đặt bàn mới
        const newReservation = new customer({
            customerName,
            phoneNumber,
            numberOfGuests,
            reservationDate: date,
            reservationTime,
            tableNumber,
        });

        // Lưu vào database
        const savedReservation = await newReservation.save();
        res.status(201).json({ message: "Reservation successful", reservation: savedReservation });

    } catch (error) {
        console.error("❌ Error making reservation:", error);
        res.status(500).json({ message: "Internal Server Error", error: error.message });
    }
}
module.exports.getReversation = async (req,res) =>{
    const reversation =  await customer.find();
    res.json(reversation)
}
module.exports.updateTable = async(req,res) =>{
    const { id } = req.params;
      const updateData = req.body;
      if (!id) {
        return res.json({
          code: 400,
          message: "Thiếu id bàn",
        });
      }
      const updatedTable = await customer.findByIdAndUpdate(id, updateData, {
        new: true, // Trả về dữ liệu sau khi cập nhật
        runValidators: true, // Kiểm tra validation của schema
      });
      if (!updatedTable) {
        return res.status(404).json({
          code: 404,
          message: "Không tìm thấy bàn để cập nhật",
        });
      }
      res.json({
        code: 200,
        message: "Cập nhật bàn thành công",
        updatedTable,
      });
}
//deleted
module.exports.deleteTable = async(req,res) =>{
    const { id } = req.params;
    const deletedTable = await customer.findByIdAndDelete(id);
    if(!deletedTable)
    {
      return  res.json({
            code: 400,
            message: "Không tìm thấy bàn để xóa"
        })
    }
    res.json({
        code:200,
        message: "Đã xóa thành công"
    })
}