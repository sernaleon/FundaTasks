<template>
  <v-container>
    <v-alert
      v-if="!$msal.isAuthenticated"
      class="d-flex align-center"
      border="top"
      colored-border
      type="info"
      elevation="2"
    >
      Welcome to Funda Tasks. Sign in to see your profile.
    </v-alert>
    <v-card
      v-if="$msal.isAuthenticated"
      class="mx-auto"
      elevation="2"
      max-width="374"
    >
      <v-card-title>Welcome to Funda Tasks!</v-card-title>
      <v-card-text>
        Tasks will go here once we wire it up to call our API!
      </v-card-text>
      <v-card-actions>
        <v-btn @click="getTasksAsync()">Refresh</v-btn>

        <v-row justify="center">
          <v-dialog v-model="dialog" persistent max-width="600px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn color="primary" dark v-bind="attrs" v-on="on">
                Add new
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">Register new task</span>
              </v-card-title>
              <v-card-text>
                <v-container>
                  <v-row>
                    <v-col cols="12">
                      <v-text-field
                        label="Name*"
                        v-model="task.name"
                        required
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                      <v-text-field
                        label="Description"
                        hint="optional"
                        v-model="task.description"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                </v-container>
                <small>*indicates required field</small>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="dialog = false"
                  >Close</v-btn
                >
                <v-btn color="blue darken-1" text @click="addTask()"
                  >Save</v-btn
                >
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-row>
      </v-card-actions>
      <v-card-text v-if="tasks">
        <ul>
          <li v-for="item in tasks" :key="item.id">
            {{ item.task.name }}: {{ item.description }}
          </li>
        </ul>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import tasksApi, { NewTask, TaskLineItem } from "../api/tasks";

@Component({
  components: {},
})
export default class Home extends Vue {
  public tasks: TaskLineItem[] = [];

  public task: NewTask = {
    name: "",
    description: "",
  };

  public dialog = false;

  async created() {
    if (this.$msal.isAuthenticated === false) {
      await this.$msal.signIn(); //POPUP IS BLOCKED. TRY WITH REDIRECT
    }
    await this.getTasksAsync();
  }

  async getTasksAsync() {
    this.tasks = await tasksApi.getTasksAsync();
  }

  async addTask() {
    this.tasks = await tasksApi.addTaskAsync(this.task);
    this.dialog = false;
    console.log(this.tasks, this.task);
  }
}
</script>
