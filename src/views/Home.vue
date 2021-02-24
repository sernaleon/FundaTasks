<template>
  <v-container>
    <v-card class="mx-auto" elevation="2" max-width="800">
      <v-data-table
        :headers="headers"
        :items="tasks"
        sort-by="description"
        class="elevation-1"
      >
        <template v-slot:top>
          <v-toolbar flat>
            <v-btn color="primary" @click="start()"> REFRESH </v-btn>
            <v-spacer></v-spacer>

            <v-dialog v-model="displayNewTaskDialog" max-width="500px">
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  color="primary"
                  dark
                  class="mb-2"
                  v-bind="attrs"
                  v-on="on"
                >
                  New
                </v-btn>
              </template>
              <v-card>
                <v-card-title>
                  <span class="headline">New item</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col cols="12">
                        <v-text-field
                          label="Name*"
                          v-model="newTask.name"
                          required
                        ></v-text-field>
                      </v-col>
                      <v-col cols="12">
                        <v-text-field
                          label="Description"
                          hint="optional"
                          v-model="newTask.description"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                  </v-container>
                  <small>*indicates required field</small>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="closeNewTask">
                    Cancel
                  </v-btn>
                  <v-btn color="blue darken-1" text @click="saveNewTask"> Save </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
            <v-dialog v-model="displayUpdateTaskDialog" max-width="500px">
              <v-card>
                <v-card-title>
                  <span class="headline">Update item</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col cols="12">
                        <v-text-field
                          label="Name*"
                          v-model="updateTask.name"
                          required
                        ></v-text-field>
                      </v-col>
                      <v-col cols="12">
                        <v-text-field
                          label="Description"
                          hint="optional"
                          v-model="updateTask.description"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                  </v-container>
                  <small>*indicates required field</small>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="closeUpdateTask">
                    Cancel
                  </v-btn>
                  <v-btn color="blue darken-1" text @click="saveUpdateTask"> Save </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-toolbar>
        </template>
        <template v-slot:[`item.actions`]="{ item }">
          <v-icon small class="mr-2" @click="openNewTaskDialog(item)"> mdi-plus </v-icon>
          <v-icon small class="mr-2" @click="openUpdateTaskDialog(item)"> mdi-pencil </v-icon>
          <v-icon small @click="openDeleteTaskDialog(item)"> mdi-delete </v-icon>
        </template>
        <template v-slot:no-data> </template>
      </v-data-table>
    </v-card>
  </v-container>
</template> 

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import tasksApi, { NewTask, Task, UpdateTask } from "../api/tasks";

@Component({
  components: {},
})
export default class Home extends Vue {
  
  public headers = [
    {
      text: "Name",
      align: "start",
      sortable: true,
      value: "name",
    },
    { text: "Description", value: "description" },
    { text: "Actions", value: "actions", sortable: false },
  ];

  public tasks: Task[] = [];
  
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
      this.tasks = await tasksApi.get();
    }
  }

  //New item dialog
  public displayNewTaskDialog = false;
  public newTask: NewTask = {
    name: "",
    description: "",
  };
  public defaultNewTask: NewTask = {
    name: "",
    description: "",
  };
  openNewTaskDialog(item: Task) {
    this.newTask.groupId = item.groupId;
    this.newTask.name = item.name;
    this.newTask.description = item.description;
    this.displayNewTaskDialog = true;
  }
  async saveNewTask() {
    this.tasks = await tasksApi.add(this.newTask);
    this.closeNewTask();
  }
  async closeNewTask() {
    this.displayNewTaskDialog = false;
    await Vue.nextTick();
    this.newTask = Object.assign({}, this.defaultNewTask);
  }

  //Update item dialog
  public displayUpdateTaskDialog = false;
  public updateTask: UpdateTask = {
    id:"",
    groupId:"",
    name: "",
    description: "",
  };
  public defaultUpdateTask: UpdateTask = {
    id:"",
    groupId:"",
    name: "",
    description: "",
  };
  openUpdateTaskDialog(item: Task) {
    this.updateTask.id = item.id,
    this.updateTask.groupId = item.groupId;
    this.updateTask.name = item.name;
    this.updateTask.description = item.description;
    this.displayUpdateTaskDialog = true;
  }
  async saveUpdateTask() {
    this.tasks = await tasksApi.update(this.updateTask);
    await this.closeUpdateTask();
  }
  async closeUpdateTask() {
    this.displayUpdateTaskDialog = false;
    await Vue.nextTick();
    this.newTask = Object.assign({}, this.defaultUpdateTask);
  }


}
</script>
