
(function ($) {

    //Reference to base object members
    var base = {
        _create: $.hik.jtable.prototype._create,
        _addColumnsToHeaderRow: $.hik.jtable.prototype._addColumnsToHeaderRow,
        _addCellsToRowUsingRecord: $.hik.jtable.prototype._addCellsToRowUsingRecord,
        _showEditForm: $.hik.jtable.prototype._showEditForm
    };


    //extension members
    $.extend(true, $.hik.jtable.prototype, {
        
        /////////////////////////////
        // Manually Call Edit Form //
        /////////////////////////////
        ShowEditForm: function ($row) {
            var self = this;

            self._showEditForm($row);
        },

        //////////////////////////////////////////////////////////////////
        // Overrides base method to add a 'edit command cell' to a row. //
        //////////////////////////////////////////////////////////////////
        _addCellsToRowUsingRecord: function ($row) {
            var self = this;
            base._addCellsToRowUsingRecord.apply(this, arguments);

            if (self.options.actions.updateAction != undefined) {
                if (typeof (self.options.actions.updateAction.enabled) != "undefined") {
                    if (!self.options.actions.updateAction.enabled($row.data('record'))) {
                        var rowCnt = ($row)[0].cells.length;
                        ($row)[0].cells[rowCnt - 1].outerHTML = "<td></td>";
                    }
                }
            }
            
        },
        
        /* Shows edit form for a row.
        *************************************************************************/
        _showEditForm: function ($tableRow) {
            var self = this;
            var record = $tableRow.data('record');

            //Create edit form
            var $editForm = $('<form id="jtable-edit-form" class="jtable-dialog-form jtable-edit-form" action="' + self.options.actions.updateAction + '" method="POST"></form>');
            if ( typeof (self.options.actions.updateAction.url) != "undefined") {
                $editForm = $('<form id="jtable-edit-form" class="jtable-dialog-form jtable-edit-form" action="' + self.options.actions.updateAction.url + '" method="POST"></form>');
            }

            //Create input fields
            for (var i = 0; i < self._fieldList.length; i++) {

                var fieldName = self._fieldList[i];
                var field = self.options.fields[fieldName];
                var fieldValue = record[fieldName];

                if (field.key == true) {
                    if (field.edit != true) {
                        //Create hidden field for key
                        $editForm.append(self._createInputForHidden(fieldName, fieldValue));
                        continue;
                    } else {
                        //Create a special hidden field for key (since key is be editable)
                        $editForm.append(self._createInputForHidden('jtRecordKey', fieldValue));
                    }
                }

                //Do not create element for non-editable fields
                if (field.edit == false) {
                    continue;
                }

                //Hidden field
                if (field.type == 'hidden') {
                    $editForm.append(self._createInputForHidden(fieldName, fieldValue));
                    continue;
                }

                //Create a container div for this input field and add to form
                var $fieldContainer = $('<div class="jtable-input-field-container"></div>').appendTo($editForm);

                //Create a label for input
                $fieldContainer.append(self._createInputLabelForRecordField(fieldName));

                //Create input element with it's current value
                var currentValue = self._getValueForRecordField(record, fieldName);
                $fieldContainer.append(
                    self._createInputForRecordField({
                        fieldName: fieldName,
                        value: currentValue,
                        record: record,
                        formType: 'edit',
                        form: $editForm
                    }));
            }

            self._makeCascadeDropDowns($editForm, record, 'edit');

            //Open dialog
            self._$editingRow = $tableRow;
            self._$editDiv.append($editForm).dialog('open');
            self._trigger("formCreated", null, { form: $editForm, formType: 'edit', record: record, row: $tableRow });
        },

    });

})(jQuery);