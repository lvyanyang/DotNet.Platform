/* ===========================================================
 * Bootstrap: fileinputx.js v3.1.3
 * http://jasny.github.com/bootstrap/javascript/#fileinputx
 * ===========================================================
 * Copyright 2012-2014 Arnold Daniels
 *
 * Licensed under the Apache License, Version 2.0 (the "License")
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ========================================================== */

+function ($) { "use strict";

  var isIE = window.navigator.appName == 'Microsoft Internet Explorer'

  // FILEUPLOAD PUBLIC CLASS DEFINITION
  // =================================

  var Fileinputx = function (element, options) {
    this.$element = $(element)
    
    this.$input = this.$element.find(':file')
    if (this.$input.length === 0) return

    this.name = this.$input.attr('name') || options.name

    this.$hidden = this.$element.find('input[type=hidden][name="' + this.name + '"]')
    if (this.$hidden.length === 0) {
      this.$hidden = $('<input type="hidden">').insertBefore(this.$input)
    }

    this.$preview = this.$element.find('.fileinputx-preview')
    var height = this.$preview.css('height')
    if (this.$preview.css('display') !== 'inline' && height !== '0px' && height !== 'none') {
      this.$preview.css('line-height', height)
    }
        
    this.original = {
      exists: this.$element.hasClass('fileinputx-exists'),
      preview: this.$preview.html(),
      hiddenVal: this.$hidden.val()
    }
    
    this.listen()
  }
  
  Fileinputx.prototype.listen = function() {
    this.$input.on('change.bs.fileinputx', $.proxy(this.change, this))
    $(this.$input[0].form).on('reset.bs.fileinputx', $.proxy(this.reset, this))
    
    this.$element.find('[data-trigger="fileinputx"]').on('click.bs.fileinputx', $.proxy(this.trigger, this))
    this.$element.find('[data-dismiss="fileinputx"]').on('click.bs.fileinputx', $.proxy(this.clear, this))
  },

  Fileinputx.prototype.change = function(e) {
    var files = e.target.files === undefined ? (e.target && e.target.value ? [{ name: e.target.value.replace(/^.+\\/, '')}] : []) : e.target.files
    
    e.stopPropagation()

    if (files.length === 0) {
      this.clear()
      return
    }

    this.$hidden.val('')
    this.$hidden.attr('name', '')
    this.$input.attr('name', this.name)

    var file = files[0]

    if (this.$preview.length > 0 && (typeof file.type !== "undefined" ? file.type.match(/^image\/(gif|png|jpeg)$/) : file.name.match(/\.(gif|png|jpe?g)$/i)) && typeof FileReader !== "undefined") {
      var reader = new FileReader()
      var preview = this.$preview
      var element = this.$element

      reader.onload = function(re) {
        var $img = $('<img>')
        $img[0].src = re.target.result
        files[0].result = re.target.result
        
        element.find('.fileinputx-filename').text(file.name)
        
        // if parent has max-height, using `(max-)height: 100%` on child doesn't take padding and border into account
        if (preview.css('max-height') != 'none') $img.css('max-height', parseInt(preview.css('max-height'), 10) - parseInt(preview.css('padding-top'), 10) - parseInt(preview.css('padding-bottom'), 10)  - parseInt(preview.css('border-top'), 10) - parseInt(preview.css('border-bottom'), 10))
        
        preview.html($img)
        element.addClass('fileinputx-exists').removeClass('fileinputx-new')

        element.trigger('change.bs.fileinputx', files)
      }

      reader.readAsDataURL(file)
    } else {
      this.$element.find('.fileinputx-filename').text(file.name)
      this.$preview.text(file.name)
      
      this.$element.addClass('fileinputx-exists').removeClass('fileinputx-new')
      
      this.$element.trigger('change.bs.fileinputx')
    }
  },

  Fileinputx.prototype.clear = function(e) {
    if (e) e.preventDefault()
    
    this.$hidden.val('')
    this.$hidden.attr('name', this.name)
    this.$input.attr('name', '')

    //ie8+ doesn't support changing the value of input with type=file so clone instead
    if (isIE) { 
      var inputClone = this.$input.clone(true);
      this.$input.after(inputClone);
      this.$input.remove();
      this.$input = inputClone;
    } else {
      this.$input.val('')
    }

    this.$preview.html('')
    this.$element.find('.fileinputx-filename').text('')
    this.$element.addClass('fileinputx-new').removeClass('fileinputx-exists')
    
    if (e !== undefined) {
      this.$input.trigger('change')
      this.$element.trigger('clear.bs.fileinputx')
    }
  },

  Fileinputx.prototype.reset = function() {
    this.clear()

    this.$hidden.val(this.original.hiddenVal)
    this.$preview.html(this.original.preview)
    this.$element.find('.fileinputx-filename').text('')

    if (this.original.exists) this.$element.addClass('fileinputx-exists').removeClass('fileinputx-new')
     else this.$element.addClass('fileinputx-new').removeClass('fileinputx-exists')
    
    this.$element.trigger('reset.bs.fileinputx')
  },

  Fileinputx.prototype.trigger = function(e) {
    this.$input.trigger('click')
    e.preventDefault()
  }

  
  // FILEUPLOAD PLUGIN DEFINITION
  // ===========================

  var old = $.fn.fileinputx
  
  $.fn.fileinputx = function (options) {
    return this.each(function () {
      var $this = $(this),
          data = $this.data('bs.fileinputx')
      if (!data) $this.data('bs.fileinputx', (data = new Fileinputx(this, options)))
      if (typeof options == 'string') data[options]()
    })
  }

  $.fn.fileinputx.Constructor = Fileinputx


  // FILEINPUT NO CONFLICT
  // ====================

  $.fn.fileinputx.noConflict = function () {
    $.fn.fileinputx = old
    return this
  }


  // FILEUPLOAD DATA-API
  // ==================

  $(document).on('click.fileinputx.data-api', '[data-provides="fileinputx"]', function (e) {
    var $this = $(this)
    if ($this.data('bs.fileinputx')) return
    $this.fileinputx($this.data())
      
    var $target = $(e.target).closest('[data-dismiss="fileinputx"],[data-trigger="fileinputx"]');
    if ($target.length > 0) {
      e.preventDefault()
      $target.trigger('click.bs.fileinputx')
    }
  })

}(window.jQuery);