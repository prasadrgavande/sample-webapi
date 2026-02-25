pipeline {
    agent any
    
    parameters {
        choice(
            name: 'DEPLOY_ENVIRONMENT',
            choices: ['dev', 'uat', 'prd'],
            description: 'Select deployment environment'
        )
        booleanParam(
            name: 'RUN_SONARQUBE',
            defaultValue: true,
            description: 'Run SonarQube analysis'
        )
        booleanParam(
            name: 'RUN_OWASP',
            defaultValue: true,
            description: 'Run OWASP Dependency Check'
        )
        booleanParam(
            name: 'SKIP_TESTS',
            defaultValue: false,
            description: 'Skip unit tests'
        )
    }
    
    environment {
        DOTNET_CLI_HOME = '/tmp/dotnet'
        DOTNET_ROOT = '/usr/share/dotnet'
        PROJECT_NAME = 'WebApi'
        SOLUTION_PATH = 'src'
        TEST_PROJECT = 'WebApi.Tests'
        PUBLISH_DIR = 'publish'
        SONARQUBE_SERVER = 'SonarQube'
        SONARQUBE_PROJECT_KEY = 'aspnet-webapi'
    }
    
    stages {
        stage('Cleanup Workspace') {
            steps {
                script {
                    echo "🧹 Cleaning workspace..."
                    deleteDir()
                }
            }
        }
        
        stage('Checkout Code') {
            steps {
                script {
                    echo "📥 Checking out code from GitHub..."
                    checkout([
                        $class: 'GitSCM',
                        branches: [[name: '*/main']],
                        userRemoteConfigs: [[
                            url: 'https://github.com/prasadrgavande/sample-webapi.git',
                            credentialsId: 'PrasadLocal'
                        ]]
                    ])
                    
                    // Display commit information
                    def commitHash = sh(returnStdout: true, script: 'git rev-parse --short HEAD').trim()
                    def commitAuthor = sh(returnStdout: true, script: 'git log -1 --pretty=format:"%an"').trim()
                    def commitMessage = sh(returnStdout: true, script: 'git log -1 --pretty=format:"%s"').trim()
                    
                    echo """
                    📝 Git Information:
                       Commit: ${commitHash}
                       Author: ${commitAuthor}
                       Message: ${commitMessage}
                    """
                }
            }
        }
        
        stage('Restore Dependencies') {
            steps {
                script {
                    echo "📦 Restoring NuGet packages..."
                    dir(SOLUTION_PATH) {
                        sh """
                            dotnet restore ${PROJECT_NAME}/${PROJECT_NAME}.csproj
                            dotnet restore ${TEST_PROJECT}/${TEST_PROJECT}.csproj
                        """
                    }
                }
            }
        }
        
        // stage('Build Application') {
        //     steps {
        //         script {
        //             echo "🔨 Building application..."
        //             dir(SOLUTION_PATH) {
        //                 sh """
        //                     dotnet build ${PROJECT_NAME}/${PROJECT_NAME}.csproj \
        //                         --configuration Release \
        //                         --no-restore
        //                 """
        //             }
        //         }
        //     }
        // }
        
        // stage('Run Unit Tests') {
        //     when {
        //         expression { !params.SKIP_TESTS }
        //     }
        //     steps {
        //         script {
        //             echo "🧪 Running unit tests..."
        //             dir(SOLUTION_PATH) {
        //                 sh """
        //                     dotnet test ${TEST_PROJECT}/${TEST_PROJECT}.csproj \
        //                         --configuration Release \
        //                         --no-build \
        //                         --logger "trx;LogFileName=test-results.trx" \
        //                         --collect:"XPlat Code Coverage" \
        //                         -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
        //                 """
        //             }
                    
        //             // Publish test results
        //             step([
        //                 $class: 'MSTestPublisher',
        //                 testResultsFile: "**/test-results.trx",
        //                 failOnError: true
        //             ])
        //         }
        //     }
        // }
        
        // stage('SonarQube Analysis') {
        //     when {
        //         expression { params.RUN_SONARQUBE }
        //     }
        //     steps {
        //         script {
        //             echo "🔍 Running SonarQube analysis..."
                    
        //             withSonarQubeEnv(SONARQUBE_SERVER) {
        //                 dir(SOLUTION_PATH) {
        //                     sh """
        //                         dotnet sonarscanner begin \
        //                             /k:"${SONARQUBE_PROJECT_KEY}" \
        //                             /d:sonar.host.url=\$SONAR_HOST_URL \
        //                             /d:sonar.login=\$SONAR_AUTH_TOKEN \
        //                             /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"
                                
        //                         dotnet build ${PROJECT_NAME}/${PROJECT_NAME}.csproj --configuration Release
                                
        //                         dotnet sonarscanner end /d:sonar.login=\$SONAR_AUTH_TOKEN
        //                     """
        //                 }
        //             }
        //         }
        //     }
        // }
        
        // stage('Quality Gate') {
        //     when {
        //         expression { params.RUN_SONARQUBE }
        //     }
        //     steps {
        //         script {
        //             echo "⏳ Waiting for SonarQube Quality Gate..."
        //             timeout(time: 5, unit: 'MINUTES') {
        //                 def qg = waitForQualityGate()
        //                 if (qg.status != 'OK') {
        //                     echo "⚠️  Quality Gate failed: ${qg.status}"
        //                     if (params.DEPLOY_ENVIRONMENT == 'prd') {
        //                         error "Quality Gate failed. Cannot deploy to production."
        //                     } else {
        //                         echo "⚠️  Proceeding despite Quality Gate failure (non-production environment)"
        //                     }
        //                 } else {
        //                     echo "✅ Quality Gate passed!"
        //                 }
        //             }
        //         }
        //     }
        // }
        
        // stage('OWASP Dependency Check') {
        //     when {
        //         expression { params.RUN_OWASP }
        //     }
        //     steps {
        //         script {
        //             echo "🛡️  Running OWASP Dependency Check..."
                    
        //             sh """
        //                 dependency-check \
        //                     --project "${PROJECT_NAME}" \
        //                     --scan "${WORKSPACE}/${SOLUTION_PATH}" \
        //                     --format HTML \
        //                     --format XML \
        //                     --out ${WORKSPACE}/dependency-check-report \
        //                     --suppression ${WORKSPACE}/dependency-check-suppressions.xml || true
        //             """
                    
        //             // Publish OWASP report
        //             publishHTML([
        //                 allowMissing: false,
        //                 alwaysLinkToLastBuild: true,
        //                 keepAll: true,
        //                 reportDir: 'dependency-check-report',
        //                 reportFiles: 'dependency-check-report.html',
        //                 reportName: 'OWASP Dependency Check Report'
        //             ])
                    
        //             // Check for high/critical vulnerabilities
        //             def vulnerabilities = sh(
        //                 returnStdout: true,
        //                 script: """
        //                     grep -c 'severity=\"HIGH\"\\|severity=\"CRITICAL\"' \
        //                     dependency-check-report/dependency-check-report.xml || echo "0"
        //                 """
        //             ).trim().toInteger()
                    
        //             if (vulnerabilities > 0) {
        //                 echo "⚠️  Found ${vulnerabilities} high/critical vulnerabilities"
        //                 if (params.DEPLOY_ENVIRONMENT == 'prd') {
        //                     error "High/Critical vulnerabilities found. Cannot deploy to production."
        //                 } else {
        //                     echo "⚠️  Proceeding despite vulnerabilities (non-production environment)"
        //                 }
        //             } else {
        //                 echo "✅ No high/critical vulnerabilities found"
        //             }
        //         }
        //     }
        // }
        
        // stage('Publish Application') {
        //     steps {
        //         script {
        //             echo "📤 Publishing application..."
                    
        //             // Determine configuration based on environment
        //             def configuration = params.DEPLOY_ENVIRONMENT == 'prd' ? 'Release' : 'Release'
                    
        //             dir(SOLUTION_PATH) {
        //                 sh """
        //                     dotnet publish ${PROJECT_NAME}/${PROJECT_NAME}.csproj \
        //                         --configuration ${configuration} \
        //                         --output ${WORKSPACE}/${PUBLISH_DIR} \
        //                         --no-restore \
        //                         --no-build \
        //                         /p:EnvironmentName=${params.DEPLOY_ENVIRONMENT.toUpperCase()}
        //                 """
        //             }
                    
        //             // Copy environment-specific appsettings
        //             sh """
        //                 if [ -f ${SOLUTION_PATH}/${PROJECT_NAME}/appsettings.${params.DEPLOY_ENVIRONMENT.capitalize()}.json ]; then
        //                     cp ${SOLUTION_PATH}/${PROJECT_NAME}/appsettings.${params.DEPLOY_ENVIRONMENT.capitalize()}.json \
        //                        ${WORKSPACE}/${PUBLISH_DIR}/appsettings.json
        //                 fi
        //             """
        //         }
        //     }
        // }
        
        // stage('Deploy with Ansible') {
        //     steps {
        //         script {
        //             echo "🚀 Deploying to ${params.DEPLOY_ENVIRONMENT.toUpperCase()} environment..."
                    
        //             // Approval for production deployments
        //             if (params.DEPLOY_ENVIRONMENT == 'prd') {
        //                 timeout(time: 15, unit: 'MINUTES') {
        //                     input message: 'Deploy to PRODUCTION?',
        //                           ok: 'Deploy',
        //                           submitter: 'admin,release-manager'
        //                 }
        //             }
                    
        //             // Run Ansible playbook
        //             ansiblePlaybook(
        //                 playbook: "${WORKSPACE}/ansible/playbooks/deploy-webapi.yml",
        //                 inventory: "${WORKSPACE}/ansible/inventories/${params.DEPLOY_ENVIRONMENT}.ini",
        //                 credentialsId: 'ansible-ssh-key',
        //                 extras: "-e artifact_source=${WORKSPACE}/${PUBLISH_DIR}",
        //                 colorized: true
        //             )
        //         }
        //     }
        // }
        
        // stage('Smoke Test') {
        //     steps {
        //         script {
        //             echo "🔥 Running smoke tests..."
                    
        //             // Get target server from inventory
        //             def inventoryFile = "${WORKSPACE}/ansible/inventories/${params.DEPLOY_ENVIRONMENT}.ini"
        //             def serverIP = sh(
        //                 returnStdout: true,
        //                 script: "grep ansible_host ${inventoryFile} | head -1 | awk '{print \$2}' | cut -d'=' -f2"
        //             ).trim()
                    
        //             def port = params.DEPLOY_ENVIRONMENT == 'dev' ? 5000 : (params.DEPLOY_ENVIRONMENT == 'uat' ? 5001 : 5002)
                    
        //             // Wait for application to be ready
        //             retry(5) {
        //                 sleep 5
        //                 sh "curl -f http://${serverIP}:${port}/WeatherForecast || exit 1"
        //             }
                    
        //             echo "✅ Smoke test passed! Application is responding."
        //         }
        //     }
        // }
    }
    
    post {
        success {
            script {
                echo """
                ✅ ========================================
                   DEPLOYMENT SUCCESSFUL!
                   ========================================
                   Environment: ${params.DEPLOY_ENVIRONMENT.toUpperCase()}
                   Build: #${BUILD_NUMBER}
                   Duration: ${currentBuild.durationString}
                ========================================
                """
            }
        }
        
        failure {
            script {
                echo """
                ❌ ========================================
                   DEPLOYMENT FAILED!
                   ========================================
                   Environment: ${params.DEPLOY_ENVIRONMENT.toUpperCase()}
                   Build: #${BUILD_NUMBER}
                   Check logs for details.
                ========================================
                """
            }
        }
        
        always {
            // Archive artifacts
            archiveArtifacts artifacts: "${PUBLISH_DIR}/**/*", allowEmptyArchive: true
            
            // Cleanup
            cleanWs()
        }
    }
}
