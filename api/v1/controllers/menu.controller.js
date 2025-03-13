const Menu = require("../models/Menu.model")
module.exports.postFood = async (req,res) =>{
    try {
        const { name, description, price, category, imageUrl, available, ingredients, discountCode, discountAmount } = req.body;

        // Kiểm tra dữ liệu đầu vào
        if (!name || !price || !category) {
            return res.status(400).json({ message: "Name, price, and category are required" });
        }

        // Tạo món ăn mới
        const newDish = new Menu({
            name,
            description,
            price,
            category,
            imageUrl,
            available,
            ingredients,
            discountAmount
        });

        // Lưu vào database
        const savedDish = await newDish.save();
        res.status(201).json(savedDish);

    } catch (error) {
        res.status(500).json({ message: "Internal Server Error", error });
    }
}
module.exports.menu = async(req,res) =>{
    const menuItems = await Menu.find();
    res.json(
        menuItems
    )
}
module.exports.deleteFood = async(req,res) =>{
    const { id } = req.params;
    const deletedFood = await Menu.findByIdAndDelete(id);
    if (!deletedFood) {
        return res.json({
          code: 400,
          message: "Không tìm thấy ca làm",
        });
      }
    res.json({
      code: 200,
      message:"Xóa thành công"
    })
  }
  // update food
  module.exports.updateFood = async(req,res) =>{
    const { id } = req.params;
      const updateData = req.body;
      if (!id) {
        return res.json({
          code: 400,
          message: "Thiếu id món ăn",
        });
      }
      const updatedFood = await Menu.findByIdAndUpdate(id, updateData, {
        new: true, // Trả về dữ liệu sau khi cập nhật
        runValidators: true, // Kiểm tra validation của schema
      });
      if (!updatedFood) {
        return res.status(404).json({
          code: 404,
          message: "Không tìm thấy món ăn để cập nhật",
        });
      }
      res.json({
        code: 200,
        message: "Cập nhật món ăn thành công",
        updatedFood,
      });
  }