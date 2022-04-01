$('#editModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var itemID = button.data('id')
    var itemfName = button.data('fn')
    var itemdName = button.data('dn')
    var itemRole = button.data('rid')

    var modal = $(this)
    
    modal.find('.item-id').val(itemID)
    modal.find('.item-dName').val(itemdName)
    modal.find('.item-fName').val(itemfName)
    modal.find('.item-roleID').val(itemRole)
})