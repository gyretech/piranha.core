/*global
    piranha
*/

Vue.component("page-field", {
    props: ["uid", "model", "meta"],
    methods: {
        select: function () {
            piranha.pagepicker.open(this.update);
        },
        remove: function () {
            this.model.id = null;
            this.model.page = null;
        },
        update: function (page) {

            console.log("update", page);

            this.model.id = page.id;
            this.model.page = page;

            console.log("uid", this.uid);
            console.log("title", this.model.page.title);

            // Tell parent that title has been updated
            this.$emit('update-title', {
                uid: this.uid,
                title: this.model.page.title
            });
        }
    },
    computed: {
        isEmpty: function () {
            return this.model.page == null;
        }
    },
    mounted: function() {
        this.model.getTitle = function () {
            if (this.model.page != null) {
                return this.model.page.title;
            } else {
                return "No page selected";
            }
        };
    },
    template:
        "<div class='media-field' :class='{ empty: isEmpty }'>" +
        "  <div class='media-picker'>" +
        "    <div class='btn-group float-right'>" +
        "      <button v-on:click.prevent='select' class='btn btn-primary text-center'>" +
        "        <i class='fas fa-plus'></i>" +
        "      </button>" +
        "      <button v-on:click.prevent='remove' class='btn btn-danger text-center'>" +
        "        <i class='fas fa-times'></i>" +
        "      </button>" +
        "    </div>" +
        "    <div class='card text-left'>" +
        "      <div class='card-body' v-if='isEmpty'>" +
        "        <span v-if='meta.placeholder != null' class='text-secondary'>{{ meta.placeholder }}</span>" +
        "        <span v-if='meta.placeholder == null' class='text-secondary'>&nbsp;</span>" +
        "      </div>" +
        "      <div class='card-body' v-else>" +
        "        <a href='#'>{{ model.page.title }}</a>" +
        "      </div>" +
        "    </div>" +
        "  </div>" +
        "</div>"
});
