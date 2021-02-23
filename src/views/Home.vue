<template>
  <v-container>
    <v-card
      class="mx-auto"
      elevation="2"
      max-width="800"
    >
    <v-data-table
    :headers="headers"
    :items="tasks"
    sort-by="description"
    class="elevation-1"
  >
    <template v-slot:top>
      <v-toolbar flat>
        <v-spacer></v-spacer>
        <v-dialog v-model="dialog" max-width="500px">
          <template v-slot:activator="{ on, attrs }">
            <v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on">
              {{ formTitle }}
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>
            <v-card-text>
              <v-container>
                <v-row>
                  <v-col cols="12">
                    <v-text-field
                      label="Name*"
                      v-model="newItem.name"
                      required
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12">
                    <v-text-field
                      label="Description"
                      hint="optional"
                      v-model="newItem.description"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
              <small>*indicates required field</small>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="close"> Cancel </v-btn>
              <v-btn color="blue darken-1" text @click="save"> Save </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:[`item.actions`]="{ item }">
      <v-icon small class="mr-2" @click="editItem(item)"> mdi-plus </v-icon>
    </template>
    <template v-slot:no-data>
      <v-btn color="primary" @click="start()"> Sign in to get data </v-btn>
    </template>
  </v-data-table>
    </v-card>
  </v-container>
  
</template> 

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import tasksApi, { NewTask, TaskLineItem } from "../api/tasks";

@Component({
  components: {},
})
export default class Home extends Vue {
  public tasks: TaskLineItem[] = [];
  public dialog = false;
  public headers = [
    {
      text: "Name",
      align: "start",
      sortable: true,
      value: "task.name",
    },
    { text: "Description", value: "description" },
    { text: "Actions", value: "actions", sortable: false },
  ];
  public newItemItem = -1;
  public newItem: NewTask = {
    name: "",
    description: "",
  };
  public defaultItem: NewTask = {
    name: "",
    description: "",
  };

  get formTitle() {
    return this.newItemItem === -1 ? "New Item" : "Edit Item";
  }

  @Watch("dialog")
  onPropertyChanged(value: boolean) {
    if (!value) {
      this.close();
    }
  }

  async created() {
    await this.getTasksAsync();
  }

  async start() {
    if (!this.$msal.isAuthenticated) {
      await this.$msal.signIn();
    }
    await this.getTasksAsync();
  }

  async getTasksAsync() {
    if (this.$msal.isAuthenticated) {
      this.tasks = await tasksApi.getTasksAsync();
    }
  }

  editItem(item: TaskLineItem) {
    this.newItemItem = this.tasks.indexOf(item);
    this.newItem.taskId = item.task.id;
    this.newItem.name = item.task.name;
    this.newItem.description = item.description;
    this.dialog = true;
  }

  async save() {
    this.tasks = await tasksApi.addTaskAsync(this.newItem);
    console.log("save", this.newItemItem, this.newItem, this.tasks);
    this.close();
  }

  async close() {
    this.dialog = false;
    await Vue.nextTick();
    this.newItem = Object.assign({}, this.defaultItem);
    this.newItemItem = -1;
  }
}
</script>
