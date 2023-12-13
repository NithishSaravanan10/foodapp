$(function() {
    $(".button").on("click", function() {
      var $button = $(this);
      var $parent = $button.parent(); 
      var oldValue = $parent.find('.input').val();
   
      if ($button.text() == "+") {
         var newVal = parseFloat(oldValue) + 1;
       } else {
          // Don't allow decrementing below zero
         if (oldValue > 1) {
           var newVal = parseFloat(oldValue) - 1;
           } else {
           newVal = 1;
         }
         }
       $parent.find('a.add-to-cart').attr('data-quantity', newVal);
       $parent.find('.input').val(newVal);
    });
   });